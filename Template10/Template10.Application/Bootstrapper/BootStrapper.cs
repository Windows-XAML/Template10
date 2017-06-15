using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Controls;
using Template10.Services.ExtendedSessionService;
using Template10.Services.NavigationService;
using Template10.Services.ViewService;
using Template10.Services.WindowWrapper;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Common
{
    public abstract class BootStrapper : Application, INotifyPropertyChanged, IBootStrapper
    {
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

        #region Convenience Properties

        /// <summary>
        /// There can be only one BootStrapper. This is a simple means to access it without
        /// re-casting Application.Current to Bootstrapper.
        /// </summary>
        /// <remarks>
        /// This is a convenience property forwarding the constant value
        /// available through BootStrapperHelper.Current
        /// </remarks>
        public static new IBootStrapperShared Current => BootStrapperHelper.Current;

        /// <summary>
        /// This memory-only dictionary gives developers a place to store complex types while
        /// navigating, including non-serializable types that cannot be passed as parameters.
        /// </summary>
        /// <remarks>
        /// This is a convenience property forwarding the constant value
        /// available through Template10.SessionState.Current
        /// </remarks>
        public IDictionary<string, object> SessionState { get; } = Template10.SessionState.Current;

        /// <summary>
        /// Returns the first NavigationService from all available services.
        /// Use the NaviagtionService to Navigate() from one page to another.
        /// </summary>
        /// <remarks>
        /// This is a convenience property forwarding the constant value
        /// available through NavigationServiceHelper.Default
        /// </remarks>
        public INavigationService NavigationService => NavigationServiceHelper.Default;

        #endregion

        #region Public Properties

        /// <summary>
        /// Out of the box, Template 10 sets a ModalDialog as the root element of the app.
        /// This enables a simple way to display a busy dialog by calling ModalDialog.IsModal = true;
        /// </summary>
        public ModalDialog ModalDialog => (Window.Current.Content as ModalDialog);

        /// <summary>
        /// Out of the box, Template 10 sets a ModalDialog as the root element of the app.
        /// This property allows developers to set the content of the ModalDialog.
        /// </summary>
        public UIElement ModalContent
        {
            get { return (Window.Current.Content as ModalDialog)?.ModalContent; }
            set { if (Window.Current.Content is ModalDialog d && d != null) d.ModalContent = value; }
        }

        private BootstrapperStates _currentState = BootstrapperStates.None;
        public BootstrapperStates CurrentState
        {
            get { return _currentState; }
            private set
            {
                DebugWrite($"CurrentState changed to {value}");
                CurrentStateHistory.Add($"{DateTime.Now}-{Guid.NewGuid()}", value);
                _currentState = value;
            }
        }
        public Dictionary<string, BootstrapperStates> CurrentStateHistory => new Dictionary<string, BootstrapperStates>();

        #endregion

        public BootStrapper()
        {
            BootStrapperHelper.Current = this;
        }

        protected sealed override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            DebugWrite();
            WindowWrapperHelper.Create(args);
            base.OnWindowCreated(args);
        }

        #region Application overides that Template 10 seals

        // it is the intent of Template 10 to no longer require Launched/Activated overrides, only OnStartAsync()
        protected override sealed void OnActivated(IActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(new OnStartEventArgs(e, StartKinds.Activate)); }
        protected override sealed void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(new OnStartEventArgs(e, StartKinds.Activate)); }
        protected override sealed void OnFileActivated(FileActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(new OnStartEventArgs(e, StartKinds.Activate)); }
        protected override sealed void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(new OnStartEventArgs(e, StartKinds.Activate)); }
        protected override sealed void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(new OnStartEventArgs(e, StartKinds.Activate)); }
        protected override sealed void OnSearchActivated(SearchActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(new OnStartEventArgs(e, StartKinds.Activate)); }
        protected override sealed void OnShareTargetActivated(ShareTargetActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(new OnStartEventArgs(e, StartKinds.Activate)); }
        protected sealed override void OnLaunched(LaunchActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(new OnStartEventArgs(e, StartKinds.Launch)); }
        protected sealed override void OnBackgroundActivated(BackgroundActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(new OnStartEventArgs(e, StartKinds.Background)); }

        #endregion

        #region Template 10's new Application overrides

        /// <summary>
        ///  By default, Template 10 will setup the root element to be a Template 10
        ///  Modal Dialog control. If you desire something different, you can set it here.
        /// </summary>
        public async virtual Task<UIElement> CreateRootElementAsync(IActivatedEventArgs e)
        {
            DebugWrite();

            var frame = new Frame();
            var service = await NavigationServiceHelper.CreateAsync(BackButton.Attach, frame);
            return new ModalDialog
            {
                DisableBackButtonWhenModal = true,
                Content = frame
            };
        }

        /// <summary>
        /// Prelaunch may never occur. However, it's possible that it will. It is a Windows mechanism
        /// to launch apps in the background and quickly suspend them. Because of this, developers need to
        /// handle Prelaunch scenarios if their typical launch is expensive or requires user interaction.
        /// </summary>
        /// <param name="args">IActivatedEventArgs from startup</param>
        /// <param name="continueStarting">A developer can force the typical startup pipeline. Default should be false.</param>
        /// <remarks>
        /// For Prelaunch Template 10 does not continue the typical startup pipeline by default.
        /// OnActivated will occur if the application has been prelaunched.
        /// </remarks>
        public virtual Task OnPrelaunchAsync(IActivatedEventArgs args, out bool continueStarting)
        {
            DebugWrite("Virtual");

            continueStarting = false;
            return Task.CompletedTask;
        }

        /// <summary>
        /// OnStartAsync is the one-stop-show override to handle when your app starts
        /// Template 10 will not call OnStartAsync if the app is restored from state.
        /// An app restores from state when the app was suspended and then terminated (PreviousExecutionState terminated).
        /// </summary>
        public abstract Task OnStartAsync(StartKinds startKind, IActivatedEventArgs args);

        /// <summary>
        /// OnInitializeAsync is where your app will do must-have up-front operations
        /// OnInitializeAsync will be called even if the application is restoring from state.
        /// An app restores from state when the app was suspended and then terminated (PreviousExecutionState terminated).
        /// </summary>
        public virtual Task OnInitializeAsync(IActivatedEventArgs args) => Task.CompletedTask;

        /// <summary>
        /// OnSuspendingAsync will be called when the application is suspending, but this override
        /// should only be used by applications that have application-level operations that must
        /// be completed during suspension.
        /// Using OnSuspendingAsync is a little better than handling the Suspending event manually
        /// because the asunc operations are in a single, global deferral created when the suspension
        /// begins and completed automatically when the last viewmodel has been called (including this method).
        /// </summary>
        /// <param name="prelaunchActivated">Indicates suspension during app prelaunch</param>
        public virtual Task OnSuspendingAsync(SuspendingEventArgs e) => Task.CompletedTask;

        /// <summary>
        /// The application is returning from a suspend state of some kind.
        /// </summary>
        /// <param name="s">Sender</param>
        /// <param name="e">EventArgs</param>
        /// <param name="previousExecutionState"></param>
        /// <remarks>
        /// previousExecutionState can be Terminated, which typically does not raise OnResume.
        /// This is important because the resume model changes a little in Mobile.
        /// </remarks>
        public virtual void OnResuming(object s, object e, AppExecutionState previousExecutionState) { }

        #endregion

        // private async void StartupOrchestratorAsync(IActivatedEventArgs e, StartKind kind)
        private async void StartupOrchestratorAsync(OnStartEventArgs e)
        {
            DebugWrite(e.ToString());

            // is the kind really right? we can figure it out from here
            if (kind == StartKinds.Launch && CurrentStateHistory.ContainsValue(BootstrapperStates.Launching)) kind = StartKinds.Activate;
            else if (kind == StartKinds.Launch && e.PreviousExecutionState == ApplicationExecutionState.Running) kind = StartKinds.Activate;
            else if (kind == StartKinds.Activate && e.PreviousExecutionState != ApplicationExecutionState.Running) kind = StartKinds.Launch;

            // handle activate
            if (kind == StartKinds.Activate)
            {
                while (!CurrentStateHistory.ContainsValue(BootstrapperStates.Launched)) { /* only activate after launch */ await Task.Delay(10); }
                while (CurrentStateHistory.Count(x => x.Value == BootstrapperStates.Starting) != CurrentStateHistory.Count(x => x.Value == BootstrapperStates.Started)) { await Task.Delay(10); }
                await OperationWrapperAsync(BootstrapperStates.Starting, async () => await OnStartAsync(kind, e), BootstrapperStates.Starting);
                OperationWrapper(BootstrapperStates.Activating, () =>
                {
                    DefaultWindowStrategy.ActivateWindow(ActivateWindowSources.Activating, DefaultSplashStrategy);
                }, BootstrapperStates.Activated);
            }

            // handle first-time launch
            else if (kind == StartKinds.Launch)
            {
                CurrentState = BootstrapperStates.Launching;
                DefaultSplashStrategy.Show(e.SplashScreen, this);

                // do some one-time things
                SetupLifecycleListeners();
                SetupBackListeners();
                SetupCustomTitleBar();

                // default Unspecified extended session
                if (AutoExtendExecutionSession) await ExtendedSessionService.StartUnspecifiedAsync();

                await OperationWrapperAsync(BootstrapperStates.Initializing, async () =>
                    {
                        await OnInitializeAsync(e);
                    }, BootstrapperStates.Initialized);

                // if there no pre-existing root then generate root
                if (DefaultSplashStrategy.IsSplashVisible || Window.Current.Content == null)
                {
                    Window.Current.Content = await CreateRootElementAsync(e);
                }

                // okay, now handle launch
                var restored = false;
                var IsPrelaunch = (e as LaunchActivatedEventArgs)?.PrelaunchActivated ?? false;
                switch (e.PreviousExecutionState)
                {
                    case ApplicationExecutionState.Suspended:
                    case ApplicationExecutionState.Terminated:
                    case ApplicationExecutionState.NotRunning: // this is new UWP behavior, suspended apps report NotRunning!
                                                               // test
                        OnResuming(this, null, IsPrelaunch ? AppExecutionState.Prelaunch : AppExecutionState.Terminated);
                        if (AutoRestoreAfterTerminated && DetermineStartCause(e) == AdditionalKinds.Primary
                            || (e is ILaunchActivatedEventArgs args && args?.TileId == string.Empty))
                        {
                            await OperationWrapperAsync(BootstrapperStates.Restoring, async () =>
                            {
                                restored = await NavigationService.LoadAsync();
                            }, BootstrapperStates.Restored);
                        }
                        break;
                    default:
                        break;
                }

                // handle if pre-launch (no UI)
                if (IsPrelaunch)
                {
                    var runOnStartAsync = false;
                    await OperationWrapperAsync(BootstrapperStates.Prelaunching, async () =>
                    {
                        await OnPrelaunchAsync(e, out runOnStartAsync);
                    }, BootstrapperStates.Prelaunched);
                    if (!runOnStartAsync) return;
                }

                // handle if not restored (new launch)
                if (!restored)
                {
                    await OperationWrapperAsync(BootstrapperStates.Starting, async () =>
                    {
                        await OnStartAsync(StartKinds.Launch, e);
                    }, BootstrapperStates.Started);
                }

                // this will also hide any showing splashscreen
                DefaultWindowStrategy.ActivateWindow(ActivateWindowSources.Launching, DefaultSplashStrategy);
                CurrentState = BootstrapperStates.Launched;
            }
        }

        private void SetupBackListeners()
        {
            Services.BackButtonService.BackButtonService.BackRequested += (s, e) =>
            {
                var handled = false;
                if (ApiInformation.IsApiContractPresent(nameof(Windows.Phone.PhoneContract), 1, 0))
                {
                    if (NavigationService?.CanGoBack == true)
                    {
                        handled = true;
                    }
                }
                else
                {
                    handled = (NavigationService?.CanGoBack == false);
                }

                RaiseBackRequested(ref handled);
                e.Handled = handled;
            };
        }

        private void SetupLifecycleListeners()
        {
            Resuming += (s, e) =>
            {
                if (_firstActivationExecuted)
                {
                    PreviousExecutionState = ApplicationExecutionState.Suspended;
                    OnResuming(this, e, AppExecutionState.Suspended);
                }
                else
                {
                    PreviousExecutionState = ApplicationExecutionState.Terminated;
                    OnResuming(this, e, AppExecutionState.Terminated);
                }
                _firstActivationExecuted = true;
            };
            Suspending += async (s, e) =>
            {
                if (PrelaunchActivated)
                {
                    return;
                }

                var deferral = e.SuspendingOperation.GetDeferral();
                try
                {
                    if (AutoExtendExecutionSession)
                    {
                        // unspecified will be revoked by suspension
                        await ExtendedSessionService.StartSaveDataAsync();
                    }

                    var navs = Services.NavigationService.NavigationService.Instances.Select(x => x).Reverse();
                    foreach (INavigationService nav in navs)
                    {
                        // individual frame-level
                        await nav.SuspendingAsync();
                        if (AutoSuspendAllFrames)
                        {
                            try
                            {
                                await nav.SaveAsync();
                            }
                            catch
                            {
                                var frameState = await nav.GetFrameStateAsync();
                                if (frameState != null)
                                {
                                    frameState.Remove("CurrentPageType");
                                    frameState.Remove("CurrentPageParam");
                                    frameState.Remove("NavigateState");
                                }
                                Exit();
                            }
                        }
                    }

                    // application-level
                    await OnSuspendingAsync(e);
                }
                finally
                {
                    ExtendedSessionService.Dispose();
                    deferral.Complete();
                }
            };
        }

        private void SetupCustomTitleBar()
        {
            // this needless test is important due to a platform bug
            try
            {
                if (Application.Current.Resources.ContainsKey("ExtendedSplashBackground"))
                {
                    var unused = Application.Current.Resources["ExtendedSplashBackground"];
                }
            }
            catch { /* this is okay */ }

            // this wonky style of loop is important due to a platform bug
            int count = Application.Current.Resources.Count;
            foreach (var resource in Application.Current.Resources)
            {
                var key = resource.Key;
                if (key == typeof(CustomTitleBar))
                {
                    var style = resource.Value as Style;
                    var title = new CustomTitleBar();
                    title.Style = style;
                }
                count--;
                if (count == 0) break;
            }
        }

        void OperationWrapper(BootstrapperStates before, Action method, BootstrapperStates after)
        {
            CurrentState = before;
            try { method(); }
            finally { CurrentState = after; }
        }

        async Task OperationWrapperAsync(BootstrapperStates before, Func<Task> method, BootstrapperStates after)
        {
            CurrentState = before;
            try { await method(); }
            finally { CurrentState = after; }
        }
    }
}
