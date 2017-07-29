using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Template10.Portable.Common;
using Template10.StartArgs;
using Template10.Extensions;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Strategies
{
    public partial class DefaultBootStrapperStrategy : IBootStrapperStrategy
    {
        // event handlers

        public async void HandleResuming(object sender, object e)
        {
            DebugWrite();
            await Template10.Settings.SuspendResumeStrategy.ResumingAsync();
            Template10.Settings.MessengerService.Send(new Messages.ResumingMessage());
        }
        public async void HandleSuspending(object sender, SuspendingEventArgs e)
        {
            DebugWrite();
            var deferral = e.SuspendingOperation.GetDeferral();
            try
            {
                await OperationWrapperAsync(BootstrapperStates.Suspending, async () =>
                {
                    await Template10.Settings.SuspendResumeStrategy?.SuspendAsync(e);
                    await Template10.Settings.ExtendedSessionStrategy?.SuspendingAsync();
                    Template10.Settings.MessengerService.Send(new Messages.SuspendingMessage { EventArgs = e });
                }, BootstrapperStates.Suspended);
            }
            finally
            {
                Template10.Settings.ExtendedSessionStrategy.Dispose();
                deferral.Complete();
            }
        }
        public void HandleEnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            DebugWrite();
            Template10.Settings.MessengerService.Send(new Messages.EnteredBackgroundMessage { EventArgs = e });
        }
        public void HandleLeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            DebugWrite();
            Template10.Settings.MessengerService.Send(new Messages.LeavingBackgroundMessage { EventArgs = e });
        }
        public void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            DebugWrite();
            Template10.Settings.MessengerService.Send(new Messages.UnhandledExceptionMessage { EventArgs = e });
        }

        // public methods

        public async Task<UIElement> CreateRootAsync(ITemplate10StartArgs e)
        {
            DebugWrite();
            return await new Frame().CreateNavigationService();
        }
        public async Task<UIElement> CreateSpashAsync(SplashScreen e)
        {
            DebugWrite();
            return null;
        }
        public void OnWindowCreated(WindowCreatedEventArgs args)
        {
            DebugWrite();
            Core.WindowEx.Create(args);
            SetupAfterFirstWindow();
        }

        public async Task<bool> ShowSplashAsync(ITemplate10StartArgs e)
        {
            var splash = await CreateSpashAsync(e.LaunchActivatedEventArgs.SplashScreen);
            if (splash == null)
            {
                DebugWrite("No splash to show.");
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
                DebugWrite("No splash to hide.");
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

        public Func<ITemplate10StartArgs, Task> OnStartAsyncDelegate { get; set; } = null;

        ValueWithHistory<BootstrapperStates> _status = new ValueWithHistory<BootstrapperStates>(BootstrapperStates.None, (date, before, after) =>
        {
            DebugWrite($"{nameof(Status)} changed from {before} to {after}");
        });
        public BootstrapperStates Status
        {
            set => _status.Value = value;
            get => _status.Value;
        }

        bool _setup = false;
        private void SetupAfterFirstWindow()
        {
            if (_setup) return;
            else _setup = true;

            DebugWrite();
            Services.BackButtonService.BackButtonService.GetInstance().BackRequested += (s, e) =>
            {
                DebugWrite();
                Template10.Settings.MessengerService.Send(new Messages.BackRequestedMessage { });
            };
        }

        // core logic

        UIElement _rootElement;
        static SemaphoreSlim StartOrchestrationAsyncSemaphore = new SemaphoreSlim(1, 1);
        public async void StartOrchestrationAsync(IActivatedEventArgs e, Template10StartArgs.StartKinds kind)
        {
            await StartOrchestrationAsyncSemaphore.WaitAsync();

            try
            {
                DebugWrite();
                var args = Template10StartArgs.Create(e, kind);

                if (args.ThisIsFirstStart)
                {
                    if (Navigation.Settings.PersistedDictionaryFactory == null)
                    {
                        Navigation.Settings.PersistedDictionaryFactory = new DefaultPersistenceStrategyFactory();
                    }

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
                            if (Template10.Settings.SuspendResumeStrategy.IsResuming(args))
                            {
                                await OperationWrapperAsync(BootstrapperStates.Restoring, async () =>
                                {
                                    var strategy = Template10.Settings.SuspendResumeStrategy;
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

                    Template10.Settings.TitleBarStrategy?.Startup();

                    await Template10.Settings.ExtendedSessionStrategy?.StartupAsync(args);
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
                DebugWrite($"Exception:{ex.Message}");
                throw;
            }
            finally
            {
                StartOrchestrationAsyncSemaphore.Release();
                Window.Current.Activate();
            }
        }
    }

    public partial class DefaultBootStrapperStrategy
    {
        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = Services.LoggingService.Severities.Template10, [CallerMemberName]string caller = null)
            => Services.LoggingService.LoggingService.WriteLine(text, severity, caller: $"{nameof(DefaultBootStrapperStrategy)}.{caller}");

        // internal

        void OperationWrapper(BootstrapperStates before, Action method, BootstrapperStates after)
        {
            Status = before;
            try { method(); }
            catch (Exception ex) { DebugWrite($"While {before}, Exception {ex.Message}"); }
            finally { Status = after; }
        }

        async Task OperationWrapperAsync(BootstrapperStates before, Func<Task> method, BootstrapperStates after)
        {
            Status = before;
            try { await method(); }
            catch (Exception ex) { DebugWrite($"While {before}, Exception {ex.Message}"); }
            finally { Status = after; }
        }
    }
}
