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
using Template10.Services.BackButtonService;

namespace Template10.Strategies
{
    public partial class DefaultBootStrapperStrategy : IBootStrapperStrategy
    {
        ILifecycleStrategy _lifecycleStrategy;
        IMessengerService _messengerService;
        IExtendedSessionStrategy _extendedSessionStrategy;
        IBackButtonService _backButtonService;
        ITitleBarStrategy _titleBarStrategy;
        public DefaultBootStrapperStrategy(
            ILifecycleStrategy lifecycleStrategy,
            IMessengerService messengerService,
            IExtendedSessionStrategy extendedSessionStrategy,
            IBackButtonService backButtonService,
            ITitleBarStrategy titleBarStrategy)
        {
            _lifecycleStrategy = lifecycleStrategy;
            _messengerService = messengerService;
            _extendedSessionStrategy = extendedSessionStrategy;
            _backButtonService = backButtonService;
            _titleBarStrategy = titleBarStrategy;
            _status = new ValueWithHistory<BootstrapperStates>(BootstrapperStates.None, (date, before, after) =>
            {
                LogThis($"{nameof(Status)} changed from {before} to {after}");
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
                }, BootstrapperStates.Suspended);
            }
            finally
            {
                _extendedSessionStrategy.Dispose();
                deferral.Complete();
            }
        }
        public void HandleEnteredBackground(object sender, EnteredBackgroundEventArgs e)
            => LogThis(() => _messengerService.Send(new Messages.EnteredBackgroundMessage { EventArgs = e }));
        public void HandleLeavingBackground(object sender, LeavingBackgroundEventArgs e)
            => LogThis(() => _messengerService.Send(new Messages.LeavingBackgroundMessage { EventArgs = e }));
        public void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
            => LogThis(() => _messengerService.Send(new Messages.UnhandledExceptionMessage { EventArgs = e }));

        // public methods

        public async Task<UIElement> CreateRootAsync(IStartArgsEx e)
        {
            LogThis();
            if (await CreateRootElementAsyncDelegate?.Invoke(e) is UIElement result && result != null)
            {
                return result;
            }
            else
            {
                var frame = await new Frame().CreateNavigationService();
                frame.GetNavigationService().FrameEx.FrameId = "RootFrame";
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

        public async Task<UIElement> CreateSpashAsync(SplashScreen e)
            => await LogThis(async () => await CreateSpashAsyncDelegate?.Invoke(e));

        public async Task<bool> ShowSplashAsync(IStartArgsEx e)
        {
            var splash = await CreateSpashAsync(e.LaunchActivatedEventArgs.SplashScreen);
            if (splash == null)
            {
                LogThis("No splash to show.");
                return false;
            }
            else
            {
                OperationWrapper(BootstrapperStates.ShowingSplash, () =>
                {
                    var window = Window.Current;
                    window.Content = splash;
                    window.Activate();
                }, BootstrapperStates.ShowedSplash);
                return true;
            }
        }

        public bool HideSplash()
        {
            var window = Window.Current;
            if (window.Content.Equals(_rootElement))
            {
                LogThis("No splash to hide.");
                return false;
            }
            else
            {
                OperationWrapper(BootstrapperStates.HidingSplash, () =>
                {
                    window.Content = _rootElement;
                    window.Activate();
                }, BootstrapperStates.HiddenSplash);
                return true;
            }
        }

        // public properties

        public Func<IStartArgsEx, Task<UIElement>> CreateRootElementAsyncDelegate { get; set; }

        public Func<SplashScreen, Task<UIElement>> CreateSpashAsyncDelegate { get; set; }

        public Func<IStartArgsEx, Task> OnStartAsyncDelegate { get; set; } = null;

        public Func<Task> OnInitAsyncDelegate { get; set; } = null;

        ValueWithHistory<BootstrapperStates> _status;
        public BootstrapperStates Status
        {
            set => _status.Value = value;
            get => _status.Value;
        }

        // core logic

        UIElement _rootElement;
        static SemaphoreSlim StartOrchestrationAsyncSemaphore = new SemaphoreSlim(1, 1);
        public async void StartOrchestrationAsync(IActivatedEventArgs e, StartArgsEx.StartKinds kind)
        {
            LogThis($"Type:{e} Kind:{kind}");
            await StartOrchestrationAsyncSemaphore.WaitAsync();

            try
            {
                LogThis();
                var args = StartArgsEx.Create(e, kind);

                if (args.ThisIsFirstStart)
                {
                    LogThis($"args.ThisIsFirstStart:{args.ThisIsFirstStart} ");
                    LogThis($"args.LaunchActivatedEventArgs?.PrelaunchActivated:{args.LaunchActivatedEventArgs?.PrelaunchActivated}");
                    LogThis($"_lifecycleStrategy.IsResuming(args):{_lifecycleStrategy.IsResuming(args)}");

                    await OperationWrapperAsync(BootstrapperStates.Initialized, async () =>
                    {
                        await OnInitAsyncDelegate?.Invoke();
                    }, BootstrapperStates.Initialized);

                    await OperationWrapperAsync(BootstrapperStates.Launching, async () =>
                    {
                        if (await ShowSplashAsync(args))
                        {
                            _rootElement = await CreateRootAsync(args);
                        }
                        else
                        {
                            var window = Window.Current;
                            window.Content = _rootElement = await CreateRootAsync(args);
                            window.Activate();
                        }

                        if (args.LaunchActivatedEventArgs?.PrelaunchActivated ?? false)
                        {
                            await OperationWrapperAsync(BootstrapperStates.Prelaunching, async () =>
                            {
                                await OnStartAsyncDelegate?.Invoke(args);
                            }, BootstrapperStates.Prelaunched);
                        }
                        else
                        {
                            var restored = false;
                            if (_lifecycleStrategy.IsResuming(args))
                            {
                                await OperationWrapperAsync(BootstrapperStates.Restoring, async () =>
                                {
                                    var strategy = _lifecycleStrategy;
                                    restored = await strategy.ResumeAsync(args);
                                }, BootstrapperStates.Restored);
                            }
                            if (!restored)
                            {
                                await OperationWrapperAsync(BootstrapperStates.Starting, async () =>
                                {
                                    await OnStartAsyncDelegate?.Invoke(args);
                                }, BootstrapperStates.Started);
                            }
                        }

                        HideSplash();

                    }, BootstrapperStates.Launched);

                    _titleBarStrategy.Update();

                    await _extendedSessionStrategy.StartupAsync(args);
                }
                else
                {
                    await OperationWrapperAsync(BootstrapperStates.Activating, async () =>
                    {
                        await OnStartAsyncDelegate?.Invoke(args);
                    }, BootstrapperStates.Activated);
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
    }

    public partial class DefaultBootStrapperStrategy : Services.Logging.Loggable
    {
        // internal

        void OperationWrapper(BootstrapperStates before, Action method, BootstrapperStates after)
        {
            Status = before;
            try { method(); }
            catch (Exception ex) { LogThis($"Error in OperationWrapper while {before}, Exception {ex.Message}", severity: Services.Logging.Severities.Error); }
            finally { Status = after; }
        }

        async Task OperationWrapperAsync(BootstrapperStates before, Func<Task> method, BootstrapperStates after)
        {
            Status = before;
            try { await method(); }
            catch (Exception ex) { LogThis($"Error in OperationWrapperAsync while {before}, Exception {ex.Message}", severity: Services.Logging.Severities.Error); }
            finally { Status = after; }
        }
    }
}
