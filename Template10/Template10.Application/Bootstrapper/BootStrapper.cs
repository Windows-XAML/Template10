using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Portable.Common;
using Template10.Services.NavigationService;
using Template10.Services.WindowWrapper;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;

namespace Template10.Common
{
    public abstract partial class BootStrapper : Application, INotifyPropertyChanged, IBootStrapper
    {
        #region IBootStrapper

        ValueWithHistory<BootstrapperStates> _status = new ValueWithHistory<BootstrapperStates>(BootstrapperStates.None, (date, before, after) =>
        {
            DebugWrite($"{nameof(Status)} changed from {before} to {after}");
        });
        /// <summary>
        /// This memory-only dictionary gives developers a place to store complex types while
        /// navigating, including non-serializable types that cannot be passed as parameters.
        /// </summary>
        /// <remarks>
        /// This is a convenience property forwarding the constant value
        /// available through Template10.Common.SessionState.Current
        /// </remarks>
        public BootstrapperStates Status
        {
            set => _status.Value = value;
            get => _status.Value;
        }

        /// <summary>
        /// This memory-only dictionary gives developers a place to store complex types while
        /// navigating, including non-serializable types that cannot be passed as parameters.
        /// </summary>
        /// <remarks>
        /// This is a convenience property forwarding the constant value
        /// available through Template10.Common.SessionState.Current
        /// </remarks>
        public IDictionary<string, object> SessionState { get; } = Template10.Common.SessionStateHelper.Current;

        /// <summary>
        /// Returns the first NavigationService from all available services.
        /// Use the NaviagtionService to Navigate() from one page to another.
        /// </summary>
        /// <remarks>
        /// This is a convenience property forwarding the constant value
        /// available through NavigationServiceHelper.Default
        /// </remarks>
        public INavigationServiceAsync NavigationService => NavigationServiceHelper.Default;

        /// <summary>
        /// OnStartAsync is the one-stop-show override to handle when your app starts
        /// Template 10 will not call OnStartAsync if the app is restored from state.
        /// An app restores from state when the app was suspended and then terminated (PreviousExecutionState terminated).
        /// </summary>
        public abstract Task OnStartAsync(Template10StartArgs e);

        #endregion

        // if start takes a while, activation can occur twice/during
        object startupLocker = new object();
        private UIElement _rootElement;

        private async void StartupOrchestratorAsync(Template10StartArgs e)
        {
            DebugWrite(e.ToString());

            using (var locker = await LockAsync.Create(startupLocker))
            {
                DoEnsurePersistedStrategyFactory();

                if (e.ThisIsFirstStart)
                {
                    // handle launch
                    await OperationWrapperAsync(BootstrapperStates.Launching, async () =>
                    {
                        ShowSplash(e);

                        _rootElement = await DoCreateRootElementAsync(e);

                        if (e.ThisIsPrelaunch)
                        {
                            await DoStartAsPrelaunch(e);
                        }
                        else
                        {
                            await DoStartAsNormal(e);
                        }

                        HideSplash();

                    }, BootstrapperStates.Launched);

                    DoSetupSuspending(e);

                    DoSetupResuming(e);

                    DoSetupTitleBar(e);

                    DoSetupBackButton(e);

                    await DoSetupExtendedSessionAsync(e);
                }
                else
                {
                    await DoStartAsActivate(e);
                }
            }
        }

        private async Task DoStartAsActivate(Template10StartArgs e)
        {
            await OperationWrapperAsync(BootstrapperStates.Activating, async () =>
            {
                await OnStartAsync(e);
            }, BootstrapperStates.Activated);
        }

        private async Task DoStartAsPrelaunch(Template10StartArgs e)
        {
            await OperationWrapperAsync(BootstrapperStates.Prelaunching, async () =>
            {
                try
                {
                    await OnStartAsync(e);
                }
                catch
                {
                    Debugger.Break();
                    throw;
                }
            }, BootstrapperStates.Prelaunched);
        }

        private async Task DoStartAsNormal(Template10StartArgs e)
        {
            if (!await DoRestoreWhenResumingAsync(e))
            {
                await OperationWrapperAsync(BootstrapperStates.Starting, async () =>
                {
                    try
                    {
                        await OnStartAsync(e);
                    }
                    catch
                    {
                        Debugger.Break();
                        throw;
                    }
                }, BootstrapperStates.Started);
            }
        }

        private static void DoEnsurePersistedStrategyFactory()
        {
            if (Settings.PersistenceStrategyFactory == null)
            {
                Settings.PersistenceStrategyFactory = new Strategies.DefaultPersistenceFactory();
            }
        }

        private void ShowSplash(Template10StartArgs e)
        {
            if (Settings.SplashFactory == null)
            {
                DebugWrite("No splash to show.");
            }
            else
            {
                OperationWrapper(BootstrapperStates.ShowingSplash, () =>
                {
                    var window = WindowWrapperHelper.Main().Window;
                    window.Content = Settings.SplashFactory(e.OnLaunchedEventArgs.SplashScreen);
                    window.Activate();
                }, BootstrapperStates.ShowedSplash);
            }
        }

        public void HideSplash()
        {
            var window = WindowWrapperHelper.Main().Window;
            if (window.Content.Equals(_rootElement))
            {
                DebugWrite("No splash to hide.");
            }
            else
            {
                OperationWrapper(BootstrapperStates.HidingSplash, () =>
                {
                    window.Content = _rootElement;
                    window.Activate();
                }, BootstrapperStates.HiddenSplash);
            }
        }

        private async Task DoSetupExtendedSessionAsync(Template10StartArgs e)
        {
            await Settings.ExtendedSessionStrategy.StartupAsync(e);
        }

        private async Task<UIElement> DoCreateRootElementAsync(Template10StartArgs e)
        {
            var root = default(UIElement);
            await OperationWrapperAsync(BootstrapperStates.CreatingRootElement, async () =>
            {
                root = await Settings.RootFactoryAsync(e);
            }, BootstrapperStates.CreatedRootElement);
            return root;
        }

        private void DoSetupTitleBar(Template10StartArgs e)
        {
            Settings.TitleBarStrategy.Startup();
        }

        private void DoSetupBackButton(Template10StartArgs e)
        {
            Services.BackButtonService.BackButtonService.Instance.BackRequested += (s, args) =>
            {
                if (args.Handled)
                {
                    return;
                }
                args.Handled = true;
                NavigationServiceHelper.Default?.GoBack();
            };
        }

        private void DoSetupResuming(Template10StartArgs args)
        {
            base.Resuming += (s, e) =>
            {
                // resume is handled in StartupOrchestrator
            };
        }

        private void DoSetupSuspending(Template10StartArgs args)
        {
            base.Suspending += async (s, e) =>
            {
                if (args.ThisIsPrelaunch)
                {
                    return;
                }

                var deferral = e.SuspendingOperation.GetDeferral();
                try
                {
                    await OperationWrapperAsync(BootstrapperStates.Suspending, async () =>
                    {
                        await Settings.ExtendedSessionStrategy.SuspendingAsync();
                        await Settings.SuspendResumeStrategy.SuspendAsync(e);

                    }, BootstrapperStates.Suspended);
                }
                finally
                {
                    Settings.ExtendedSessionStrategy.Dispose();
                    deferral.Complete();
                }
            };
        }

        private async Task<bool> DoRestoreWhenResumingAsync(Template10StartArgs e)
        {
            var restored = false;
            if (Settings.SuspendResumeStrategy.IsResuming(e))
            {
                await OperationWrapperAsync(BootstrapperStates.Resuming, async () =>
                {
                    if (Settings.RunRestoreStrategy)
                    {
                        await OperationWrapperAsync(BootstrapperStates.Restoring, async () =>
                        {
                            restored = await Settings.SuspendResumeStrategy.ResumeAsync(e);
                            DebugWrite($"After Restore. Restored: {restored}");
                        }, BootstrapperStates.Restored);
                    }
                    else
                    {
                        DebugWrite("RunRestoreStrategy == false;");
                    }
                }, BootstrapperStates.Resumed);
            }
            else
            {
                DebugWrite("Not Resuming;");
            }
            return restored;
        }
    }

    public abstract partial class BootStrapper : Application, INotifyPropertyChanged, IBootStrapper
    {
        public BootStrapper()
        {
            BootStrapperHelper.Current = this;

        }

        private new event EventHandler<object> Resuming;
        private new event SuspendingEventHandler Suspending;

        /// <summary>
        /// There can be only one BootStrapper. This is a simple means to access it without
        /// re-casting Application.Current to Bootstrapper.
        /// </summary>
        /// <remarks>
        /// This is a convenience property forwarding the constant value
        /// available through BootStrapperHelper.Current
        /// </remarks>
        public static new IBootStrapperShared Current => BootStrapperHelper.Current;

        #region Application overrides that Template 10 seals

        // it is the intent of Template 10 to no longer require Launched/Activated overrides, only OnStartAsync()
        protected override sealed void OnActivated(IActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(new Template10StartArgs(e, StartKinds.Activate)); }
        protected override sealed void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(new Template10StartArgs(e, StartKinds.Activate)); }
        protected override sealed void OnFileActivated(FileActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(new Template10StartArgs(e, StartKinds.Activate)); }
        protected override sealed void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(new Template10StartArgs(e, StartKinds.Activate)); }
        protected override sealed void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(new Template10StartArgs(e, StartKinds.Activate)); }
        protected override sealed void OnSearchActivated(SearchActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(new Template10StartArgs(e, StartKinds.Activate)); }
        protected override sealed void OnShareTargetActivated(ShareTargetActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(new Template10StartArgs(e, StartKinds.Activate)); }
        protected sealed override void OnLaunched(LaunchActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(new Template10StartArgs(e, StartKinds.Launch)); }
        protected sealed override void OnBackgroundActivated(BackgroundActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(new Template10StartArgs(e, StartKinds.Background)); }
        protected sealed override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            DebugWrite();
            var wrapper = WindowWrapperFactory.Create(args);
            base.OnWindowCreated(args);
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                RaisePropertyChanged(propertyName);
            }
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        #region Debug

        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = Services.LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            Services.LoggingService.LoggingService.WriteLine(text, severity, caller: $"BootStrapper.{caller}");

        #endregion

        void OperationWrapper(BootstrapperStates before, Action method, BootstrapperStates after)
        {
            Status = before;
            try { method(); }
            finally { Status = after; }
        }

        async Task OperationWrapperAsync(BootstrapperStates before, Func<Task> method, BootstrapperStates after)
        {
            Status = before;
            try { await method(); }
            finally { Status = after; }
        }
    }
}
