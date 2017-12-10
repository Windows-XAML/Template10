using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Template10.Portable.Common;
using Template10.Common;
using Template10.Extensions;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Template10.Services.Gesture;
using Template10.BootStrap;
using Template10.Navigation;
using Template10.Popups;
using Template10.Services.Messaging;

namespace Template10.Strategies
{
    public partial class DefaultBootStrapperStrategy : IBootStrapperStrategy
    {
        ILifecycleStrategy _lifecycleStrategy;
        IMessengerService _messengerService;
        IExtendedSessionStrategy _extendedSessionStrategy;
        IGestureService _gestureService;
        public DefaultBootStrapperStrategy(
            ILifecycleStrategy lifecycleStrategy,
            IMessengerService messengerService,
            IExtendedSessionStrategy extendedSessionStrategy,
            IGestureService gestureService)
        {
            _lifecycleStrategy = lifecycleStrategy;
            _messengerService = messengerService;
            _extendedSessionStrategy = extendedSessionStrategy;
            _gestureService = gestureService;
            _status = new ValueWithHistory<BootstrapperStates>(BootstrapperStates.None, (date, before, after) =>
            {
                this.Log($"{nameof(Status)} changed from {before} to {after}", caller: $"{nameof(DefaultBootStrapperStrategy)}");
            });
        }

        // event handlers

        public async void HandleResuming(object sender, object e)
        {
            this.Log();
            await _lifecycleStrategy.ResumingAsync();
            _messengerService.Send(new Messages.ResumingMessage());
        }

        public async void HandleSuspending(object sender, SuspendingEventArgs e)
        {
            this.Log();
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
            this.Log();
            _messengerService.Send(new Messages.EnteredBackgroundMessage { EventArgs = e });
            _messengerService.Send(new Messages.AppVisibilityChangedMessage { Visibility = AppVisibilities.Background });
        }

        public void HandleLeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            this.Log();
            _messengerService.Send(new Messages.LeavingBackgroundMessage { EventArgs = e });
            _messengerService.Send(new Messages.AppVisibilityChangedMessage { Visibility = AppVisibilities.Foreground });
        }

        public void HandleUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
            => this.Log(() => _messengerService.Send(new Messages.UnhandledExceptionMessage { EventArgs = e }));

        // public methods

        public UIElement CreateRoot(IStartArgsEx e)
        {
            this.Log();
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
            this.Log();
            var window = WindowExFactory.Create(args);
            _messengerService.Send(new Messages.WindowCreatedMessage { EventArgs = args });

            if (!_windowSetup && window.IsMainView)
            {
                _windowSetup = true;
                this.Log("OneTimeWindowSetup");
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

        static SemaphoreSlim _startOrchestrationAsyncSemaphore = new SemaphoreSlim(1, 1);
        public async void StartOrchestrationAsync(IActivatedEventArgs e, StartArgsEx.StartKinds kind)
        {
            this.Log($"Type:{e} Kind:{kind}");

            await _startOrchestrationAsyncSemaphore.WaitAsync();

            try
            {
                this.Log();
                var args = StartArgsEx.Create(e, kind);

                this.Log($"args.StartKind:{args.StartKind} ");
                this.Log($"args.StartCause:{args.StartCause} ");
                this.Log($"args.ThisIsFirstStart:{args.ThisIsFirstStart} ");
                this.Log($"args.PreviousExecutionState:{args.PreviousExecutionState} ");
                this.Log($"args.LaunchActivatedEventArgs?.PrelaunchActivated:{args.LaunchActivatedEventArgs?.PrelaunchActivated}");
                this.Log($"_lifecycleStrategy.IsResuming(args):{_lifecycleStrategy.IsResuming(args)}");

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
                this.Log($"Exception:{ex.Message}");
                throw;
            }
            finally
            {
                _startOrchestrationAsyncSemaphore.Release();
                Window.Current.Activate();
            }
        }
    }

    public partial class DefaultBootStrapperStrategy 
    {
        // internal

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
                if (Template10.Settings.SplashShowBehavior == SplashShowBehaviors.Auto)
                {
                    OperationWrapper(BootstrapperStates.ShowingSplash, () =>
                    {
                        splash.Data.SplashScreen = args.LaunchActivatedEventArgs.SplashScreen;
                        splash.IsShowing = true;
                    }, BootstrapperStates.ShowedSplash, "Problem setting SplashPopup.IsShowing=true;");
                }
                else
                {
                    this.Log("SplashPopup is set to SplashShowBehavior=Auto;, developer will show it.");
                }
            }
            else
            {
                this.Log("SplashPopup is not found, nothing to show.");
            }
        }

        private void HideSplash()
        {
            if (PopupsExtensions.TryGetPopup<SplashPopup>(out var splash))
            {
                if (Template10.Settings.SplashHideBehavior == SplashHideBehaviors.Auto)
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
                        this.Log("SplashPopup is not showing, nothing to hide.");
                    }
                }
                else
                {
                    this.Log("SplashPopup is set to SplashHideBehavior=Manual;, developer will hide it.");
                }
            }
            else
            {
                this.Log("SplashPopup is not found, nothing to hide.");
            }
        }

        private void OperationWrapper(BootstrapperStates before, Action method, BootstrapperStates after, string message)
        {
            Status = before;
            try { method(); }
            catch (Exception ex)
            {
                // message += $"\r\nError in {GetType()}.{nameof(OperationWrapper)} while {before}. Exception:{ex.Message}";
                this.Log(message, severity: Services.Logging.Severities.Error);
                throw new Exception(message, ex);
            }
            finally { Status = after; }
        }

        private async Task OperationWrapperAsync(BootstrapperStates before, Func<Task> method, BootstrapperStates after, string message)
        {
            Status = before;
            try { await method(); }
            catch (Exception ex)
            {
                // message += $"\r\nError in {GetType()}.{nameof(OperationWrapperAsync)} while {before}. Exception:{ex.Message}";
                this.Log(message, severity: Services.Logging.Severities.Error);
                throw new Exception(message, ex);
            }
            finally { Status = after; }
        }
    }
}
