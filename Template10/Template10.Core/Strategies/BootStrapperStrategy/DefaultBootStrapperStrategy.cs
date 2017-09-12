using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Template10.Portable.Common;
using Template10.Core;
using Template10.Extensions;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Template10.Services.Messenger;
using Template10.Services.Gesture;
using Template10.BootStrap;
using Template10.Navigation;
using Template10.Popups;

namespace Template10.Strategies
{
    public partial class DefaultBootStrapperStrategy : IBootStrapperStrategy
    {
        ILifecycleStrategy _lifecycleStrategy;
        IMessengerService _messengerService;
        IExtendedSessionStrategy _extendedSessionStrategy;
        IBackButtonService _backButtonService;
        public DefaultBootStrapperStrategy(
            ILifecycleStrategy lifecycleStrategy,
            IMessengerService messengerService,
            IExtendedSessionStrategy extendedSessionStrategy,
            IBackButtonService backButtonService)
        {
            _lifecycleStrategy = lifecycleStrategy;
            _messengerService = messengerService;
            _extendedSessionStrategy = extendedSessionStrategy;
            _backButtonService = backButtonService;
            _status = new ValueWithHistory<BootstrapperStates>(BootstrapperStates.None, (date, before, after) =>
            {
                LogThis($"{nameof(Status)} changed from {before} to {after}", caller: $"{nameof(DefaultBootStrapperStrategy)}");
            });
        }

        // event handlers

        public async void HandleResuming(object sender, object e)
        {
            LogThis();
            await _lifecycleStrategy.ResumingAsync();
            _messengerService.Send(new Messages.ResumingMessage());
        }

        public async void HandleSuspending(object sender, SuspendingEventArgs e)
        {
            LogThis();
            var deferral = e.SuspendingOperation.GetDeferral();
            try
            {
                await OperationWrapperAsync(BootstrapperStates.Suspending, async () =>
                {
                    await _lifecycleStrategy.SuspendAsync(e);
                    await _extendedSessionStrategy.SuspendingAsync();
                    _messengerService.Send(new Messages.SuspendingMessage { EventArgs = e });
                }, BootstrapperStates.Suspended, "Problem in the LifecycleStrategy.Suspend/Suspending implementation.");
            }
            finally
            {
                _extendedSessionStrategy.Dispose();
                deferral.Complete();
            }
        }

        public void HandleEnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            LogThis();
            _messengerService.Send(new Messages.EnteredBackgroundMessage { EventArgs = e });
            _messengerService.Send(new Messages.AppVisibilityChangedMessage { Visibility = AppVisibilities.Background });
        }

        public void HandleLeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            LogThis();
            _messengerService.Send(new Messages.LeavingBackgroundMessage { EventArgs = e });
            _messengerService.Send(new Messages.AppVisibilityChangedMessage { Visibility = AppVisibilities.Foreground });
        }

        public void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
            => LogThis(() => _messengerService.Send(new Messages.UnhandledExceptionMessage { EventArgs = e }));

        // public methods

        public UIElement CreateRoot(IStartArgsEx e)
        {
            LogThis();
            if (CreateRootElementDelegate?.Invoke(e) is UIElement result && result != null)
            {
                return result;
            }
            else
            {
                var frame = new Frame();
                try
                {
                    frame.CreateNavigationService("RootFrame");
                    frame.GetNavigationService().FrameEx.FrameId = "RootFrame";
                }
                catch (Exception)
                {
                    System.Diagnostics.Debugger.Break();
                    throw;
                }
                return frame;
            }
        }

        bool _windowSetup = false;
        public void OnWindowCreated(WindowCreatedEventArgs args)
        {
            LogThis();
            var window = WindowEx.Create(args);
            _messengerService.Send(new Messages.WindowCreatedMessage { EventArgs = args });

            if (!_windowSetup && window.IsMainView)
            {
                _windowSetup = true;
                LogThis("OneTimeWindowSetup");
                _backButtonService.BackRequested += (s, e) =>
                {
                    LogThis("BackButtonService.BackRequested");
                    _messengerService.Send(new Messages.BackRequestedMessage { });
                };
            }
        }

        // public properties

        public Func<IStartArgsEx, UIElement> CreateRootElementDelegate { get; set; }

        public Func<IStartArgsEx, INavigationService, ISessionState, Task> OnStartAsyncDelegate { get; set; } = null;

        public Func<Task> OnInitAsyncDelegate { get; set; } = null;

        ValueWithHistory<BootstrapperStates> _status;
        public BootstrapperStates Status
        {
            set => _status.Value = value;
            get => _status.Value;
        }

        // core logic

        static SemaphoreSlim StartOrchestrationAsyncSemaphore = new SemaphoreSlim(1, 1);
        public async void StartOrchestrationAsync(IActivatedEventArgs e, StartArgsEx.StartKinds kind)
        {
            LogThis($"Type:{e} Kind:{kind}");

            await StartOrchestrationAsyncSemaphore.WaitAsync();

            try
            {
                LogThis();
                var args = StartArgsEx.Create(e, kind);

                LogThis($"args.StartKind:{args.StartKind} ");
                LogThis($"args.StartCause:{args.StartCause} ");
                LogThis($"args.ThisIsFirstStart:{args.ThisIsFirstStart} ");
                LogThis($"args.PreviousExecutionState:{args.PreviousExecutionState} ");
                LogThis($"args.LaunchActivatedEventArgs?.PrelaunchActivated:{args.LaunchActivatedEventArgs?.PrelaunchActivated}");
                LogThis($"_lifecycleStrategy.IsResuming(args):{_lifecycleStrategy.IsResuming(args)}");

                if (args.ThisIsFirstStart)
                {
                    await OperationWrapperAsync(BootstrapperStates.Initializing, async () =>
                    {
                        await OnInitAsyncDelegate?.Invoke();
                    }, BootstrapperStates.Initialized, "Problem in your custom OnInitizedAsync() implementation.");

                    await OperationWrapperAsync(BootstrapperStates.Launching, async () =>
                    {
                        ShowSplash(args);

                        OperationWrapper(BootstrapperStates.CreatingRootElement, () =>
                        {
                            Window.Current.Content = CreateRoot(args);
                        }, BootstrapperStates.CreatedRootElement, "Problem in your custom CreateRoot() implementation.");

                        if (args.LaunchActivatedEventArgs?.PrelaunchActivated ?? false)
                        {
                            await OperationWrapperAsync(BootstrapperStates.Prelaunching, async () =>
                            {
                                await OnStartAsyncDelegate?.Invoke(args, NavigationService.Default, Central.SessionState);
                            }, BootstrapperStates.Prelaunched, "Problem in your custom OnStartAsync() implementation.");
                        }
                        else
                        {
                            var restored = false;
                            restored = await Restore(args);

                            if (!restored)
                            {
                                await OperationWrapperAsync(BootstrapperStates.Starting, async () =>
                                {
                                    await OnStartAsyncDelegate?.Invoke(args, NavigationService.Default, Central.SessionState);
                                }, BootstrapperStates.Started, "Problem in your custom OnStartAsync() implementation.");
                            }
                        }

                        HideSplash();

                    }, BootstrapperStates.Launched, "Problem in the subprocesses of Launch.");

                    await _extendedSessionStrategy.StartupAsync(args);
                }
                else
                {
                    await OperationWrapperAsync(BootstrapperStates.Activating, async () =>
                    {
                        await OnStartAsyncDelegate?.Invoke(args, NavigationService.Default, Central.SessionState);
                    }, BootstrapperStates.Activated, "Problem in your custom OnStartAsync() implementation.");
                }
            }
            catch (Exception ex)
            {
                LogThis($"Exception:{ex.Message}");
                throw;
            }
            finally
            {
                StartOrchestrationAsyncSemaphore.Release();
                Window.Current.Activate();
            }
        }

        private async Task<bool> Restore(IStartArgsEx args)
        {
            var restored = false;
            if (_lifecycleStrategy.IsResuming(args))
            {
                await OperationWrapperAsync(BootstrapperStates.Restoring, async () =>
                {
                    restored = await _lifecycleStrategy.ResumeAsync(args);
                }, BootstrapperStates.Restored, "Problem in the LifecycleStrategy.ResumeAsync() implementation.");
            }
            return restored;
        }

        private void ShowSplash(IStartArgsEx args)
        {
            if (PopupsExtensions.TryGetPopup<SplashPopup>(out var splash))
            {
                if (splash.AutoShow)
                {
                    OperationWrapper(BootstrapperStates.ShowingSplash, () =>
                    {
                        splash.Content.SplashScreen = args.LaunchActivatedEventArgs.SplashScreen;
                        splash.IsShowing = true;
                    }, BootstrapperStates.ShowedSplash, "Problem setting SplashPopup.IsShowing=true;");
                }
                else
                {
                    LogThis("SplashPopup is set to AutoShow=false;, developer will show it.");
                }
            }
            else
            {
                LogThis("SplashPopup is not found, nothing to show.");
            }
        }

        private void HideSplash()
        {
            if (PopupsExtensions.TryGetPopup<SplashPopup>(out var splash))
            {
                if (splash.AutoHide)
                {
                    if (splash.IsShowing)
                    {
                        OperationWrapper(BootstrapperStates.HidingSplash, () =>
                        {
                            splash.IsShowing = false;
                        }, BootstrapperStates.HiddenSplash, "Problem setting SplashPopup.IsShowing=false;");
                    }
                    else
                    {
                        LogThis("SplashPopup is not showing, nothing to hide.");
                    }
                }
                else
                {
                    LogThis("SplashPopup is set to AutoHide=false;, developer will hide it.");
                }
            }
            else
            {
                LogThis("SplashPopup is not found, nothing to hide.");
            }
        }
    }

    public partial class DefaultBootStrapperStrategy : Services.Logging.Loggable
    {
        // internal

        void OperationWrapper(BootstrapperStates before, Action method, BootstrapperStates after, string message)
        {
            Status = before;
            try { method(); }
            catch (Exception ex)
            {
                // message += $"\r\nError in {GetType()}.{nameof(OperationWrapper)} while {before}. Exception:{ex.Message}";
                LogThis(message, severity: Services.Logging.Severities.Error);
                throw new Exception(message, ex);
            }
            finally { Status = after; }
        }

        async Task OperationWrapperAsync(BootstrapperStates before, Func<Task> method, BootstrapperStates after, string message)
        {
            Status = before;
            try { await method(); }
            catch (Exception ex)
            {
                // message += $"\r\nError in {GetType()}.{nameof(OperationWrapperAsync)} while {before}. Exception:{ex.Message}";
                LogThis(message, severity: Services.Logging.Severities.Error);
                throw new Exception(message, ex);
            }
            finally { Status = after; }
        }
    }
}
