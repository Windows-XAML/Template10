using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Template10.Services.ViewService;
using System.Threading;
using Windows.ApplicationModel.ExtendedExecution;
using Template10.Services.ExtendedSessionService;
using Template10.Utils;
using System.Diagnostics;

namespace Template10.Common
{
    public abstract class BootStrapper : Application, INotifyPropertyChanged
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

        #region Public Enums

        public enum StartKind { Launch, Activate }

        public enum AppExecutionState { Suspended, Terminated, Prelaunch }

        public enum BackButton { Attach, Ignore }

        public enum ExistingContent { Include, Exclude }

        public enum AdditionalKinds { Primary, Toast, SecondaryTile, Other, JumpListItem }

        #endregion

        #region Public Properties

        /// <summary>
        /// There can be only one BootStrapper. This is a simple means to access it without
        /// re-casting Application.Current to Bootstrapper.
        /// </summary>
        public static new BootStrapper Current => Application.Current as BootStrapper;

        /// <summary>
        /// This memory-only dictionary gives developers a place to store complex types while 
        /// navigating, including non-serializable types that cannot be passed as parameters.
        /// </summary>
        public IStateItems SessionState { get; set; } = new StateItems();

        /// <summary>
        /// This property tells Template 10 if should automatically restore the NavitgationState
        /// of Frames when the application is restored from suspension. 
        /// </summary>
        public bool AutoRestoreAfterTerminated { get; set; } = true;

        /// <summary>
        /// This setting tells Template 10 if it should automatically implement a SavingData 
        /// ExtendedSession when Suspending. This extends the time limit for Suspension activity.
        /// </summary>
        public bool AutoExtendExecutionSession { get; set; } = true;

        /// <summary>
        /// This setting tells Template 10 if it should automatically save the NavigationState
        /// of every NavigationService's Frame. This enables it to be restored on Resume. 
        /// </summary>
        public bool AutoSuspendAllFrames { get; set; } = true;

        /// <summary>
        /// There are many ways for an app to start and re-start, including activation.
        /// This property retains the original (first-launch) activation arguments. 
        /// </summary>
        public IActivatedEventArgs OriginalActivatedArgs { get; private set; }

        /// <summary>
        /// Out of the box, Template 10 sets a ModalDialog as the root element of the app.
        /// This enables a simple way to display a busy dialog by calling ModalDialog.IsModal = true;
        /// </summary>
        public Controls.ModalDialog ModalDialog => (Window.Current.Content as Controls.ModalDialog);

        /// <summary>
        /// Out of the box, Template 10 sets a ModalDialog as the root element of the app.
        /// This property allows developers to set the content of the ModalDialog.
        /// </summary>
        public UIElement ModalContent
        {
            get { return ModalDialog?.ModalContent; }
            set { if (ModalDialog != null) ModalDialog.ModalContent = value; }
        }

        /// <summary>
        /// Returns the first NavigationService from all available services.
        /// Use the NaviagtionService to Navigate() from one page to another.
        /// </summary>
        public INavigationService NavigationService => WindowWrapper.Current().NavigationServices.FirstOrDefault();

        /// <summary>
        /// The SplashFactory is a Func that returns an instantiated Splash view.
        /// Template 10 will automatically inject this visual before loading the app.
        /// </summary>
        public Func<SplashScreen, UserControl> SplashFactory { get; protected set; }

        /// <summary>
        /// CacheMaxDuration indicates the maximum TimeSpan for which cache data
        /// will be preserved. If Template 10 determines cache data is older than
        /// the specified MaxDuration it will automatically be cleared during start.
        /// </summary>
        public TimeSpan CacheMaxDuration { get; set; } = TimeSpan.MaxValue;
        private const string CacheDateKey = "Setting-Cache-Date";

        /// <summary>
        /// ShowShellBackButton is used to show or hide the shell-drawn back button that
        /// is new to Windows 10. A developer can do this manually, but using this property
        /// is important during navigation because Template 10 manages the visibility
        /// of the shell-drawn back button at that time.
        /// </summary>
        public bool ShowShellBackButton { get; set; } = true;

        public bool ForceShowShellBackButton { get; set; } = false;

        [Obsolete("This will be made private in future versions.")]
        public const string DefaultTileID = "App";

        [Obsolete("Use AutoRestoreAfterTerminated")]
        public bool EnableAutoRestoreAfterTerminated { get; set; } = true;

        #endregion

        Lazy<WindowLogic> _WindowLogic;
        internal WindowLogic WindowLogic => _WindowLogic.Value;

        Lazy<SplashLogic> _SplashLogic;
        internal SplashLogic SplashLogic => _SplashLogic.Value;

        Lazy<ExtendedSessionService> _ExtendedSessionService;
        private ExtendedSessionService ExtendedSessionService => _ExtendedSessionService.Value;

        public BootStrapper()
        {
            _WindowLogic = new Lazy<WindowLogic>(() => new WindowLogic());
            _SplashLogic = new Lazy<SplashLogic>(() => new SplashLogic());
            _ExtendedSessionService = new Lazy<ExtendedSessionService>(() => new ExtendedSessionService());
        }

        #region Public Events

        public event EventHandler<WindowCreatedEventArgs> WindowCreated;
        protected sealed override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            DebugWrite();

            // setup window wrapper
            new WindowWrapper(args.Window);
            ViewService.OnWindowCreated();
            WindowCreated?.Invoke(this, args);
            base.OnWindowCreated(args);
        }

        public static event EventHandler<HandledEventArgs> BackRequested;
        private void RaiseBackRequested(ref bool handled)
        {
            DebugWrite();

            var args = new HandledEventArgs();
            BackRequested?.Invoke(null, args);
            if (handled = args.Handled)
            {
                return;
            }
            foreach (var navigationService in WindowWrapper.Current().NavigationServices.Select(x => x).Reverse())
            {
                navigationService.RaiseBackRequested(args);
                if (handled = args.Handled)
                {
                    return;
                }
            }
            handled = (NavigationService.FrameFacade.BackStack.Count > 0);
            NavigationService.GoBack();
        }

        public static event EventHandler<HandledEventArgs> ForwardRequested;
        private void RaiseForwardRequested()
        {
            DebugWrite();

            var args = new HandledEventArgs();
            ForwardRequested?.Invoke(null, args);
            if (args.Handled)
            {
                return;
            }
            foreach (var navigationService in WindowWrapper.Current().NavigationServices)
            {
                navigationService.RaiseForwardRequested(args);
                if (args.Handled)
                {
                    return;
                }
            }
            NavigationService.GoForward();
        }

        public event EventHandler ShellBackButtonUpdated;
        private void RaiseShellBackButtonUpdated()
        {
            ShellBackButtonUpdated?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Public Methods

        public void UpdateShellBackButton()
        {
            DebugWrite();

            // show the shell back only if there is anywhere to go in the default frame
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                (ShowShellBackButton && (NavigationService.CanGoBack || ForceShowShellBackButton))
                    ? AppViewBackButtonVisibility.Visible
                    : AppViewBackButtonVisibility.Collapsed;
            RaiseShellBackButtonUpdated();
        }

        /// <summary>
        ///  By default, Template 10 will setup the root element to be a Template 10
        ///  Modal Dialog control. If you desire something different, you can set it here.
        /// </summary>
        public virtual UIElement CreateRootElement(IActivatedEventArgs e)
        {
            DebugWrite();

            var navigationFrame = new Frame();
            var navigationService = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include, navigationFrame);
            return new Controls.ModalDialog
            {
                DisableBackButtonWhenModal = true,
                Content = navigationFrame
            };
        }

        /// <summary>
        /// Creates a new NavigationService from the gived Frame to the 
        /// WindowWrapper collection. In addition, it optionally will setup the 
        /// shell back button to react to the nav of the Frame.
        /// A developer should call this when creating a new/secondary frame.
        /// The shell back button should only be setup one time.
        /// </summary>
        public INavigationService NavigationServiceFactory(BackButton backButton, ExistingContent existingContent, Frame frame = null)
        {
            DebugWrite($"{nameof(backButton)}:{backButton} {nameof(existingContent)}:{existingContent} {nameof(frame)}:{frame}");

            (frame = frame ?? new Frame()).Content = (existingContent == ExistingContent.Include) ? Window.Current.Content : null;

            // if the service already exists for this frame, use the existing one.
            foreach (var nav in WindowWrapper.ActiveWrappers.SelectMany(x => x.NavigationServices))
            {
                if (nav.FrameFacade.Frame.Equals(frame))
                {
                    return nav as INavigationService;
                }
            }

            var navigationService = new NavigationService(frame);
            navigationService.BackButtonHandling = backButton;
            WindowWrapper.Current().NavigationServices.Add(navigationService);

            if (backButton == BackButton.Attach)
            {
                // TODO: unattach others

                // update shell back when backstack changes
                // only the default frame in this case because secondary should not dismiss the app
                frame.RegisterPropertyChangedCallback(Frame.BackStackDepthProperty, (s, args) => UpdateShellBackButton());

                // update shell back when navigation occurs
                // only the default frame in this case because secondary should not dismiss the app
                frame.Navigated += (s, args) => UpdateShellBackButton();
            }

            // this is always okay to check, default or not
            // expire any state (based on expiry)
            DateTime cacheDate;
            // default the cache age to very fresh if not known
            var otherwise = DateTime.MinValue.ToString();
            if (DateTime.TryParse(navigationService.Suspension.GetFrameState().Read(CacheDateKey, otherwise), out cacheDate))
            {
                var cacheAge = DateTime.Now.Subtract(cacheDate);
                if (cacheAge >= CacheMaxDuration)
                {
                    // clear state in every nav service in every view
                    foreach (var nav in WindowWrapper.ActiveWrappers.SelectMany(x => x.NavigationServices))
                    {
                        nav.Suspension.ClearFrameState();
                    }
                }
            }
            else
            {
                // no date, that's okay
            }
            return navigationService;
        }


        private BootstrapperStates _currentState = BootstrapperStates.None;
        public BootstrapperStates CurrentState
        {
            get { return _currentState; }
            set
            {
                DebugWrite($"CurrenstState changed to {value}");
                CurrentStateHistory.Add($"{DateTime.Now}-{Guid.NewGuid()}", value);
                _currentState = value;
            }
        }
        Dictionary<string, BootstrapperStates> CurrentStateHistory = new Dictionary<string, BootstrapperStates>();

        /// <summary>
        /// This determines the simplest case for starting. This should handle 80% of common scenarios. 
        /// When Other is returned the developer must determine start manually using IActivatedEventArgs.Kind
        /// </summary>
        public static AdditionalKinds DetermineStartCause(IActivatedEventArgs args)
        {
            DebugWrite($"{nameof(IActivatedEventArgs)}:{args.Kind}");

            if (args is ToastNotificationActivatedEventArgs)
            {
                return AdditionalKinds.Toast;
            }
            var e = args as ILaunchActivatedEventArgs;
            if (e?.TileId == DefaultTileID && string.IsNullOrEmpty(e?.Arguments))
            {
                return AdditionalKinds.Primary;
            }
            else if (e?.TileId == DefaultTileID && !string.IsNullOrEmpty(e?.Arguments))
            {
                return AdditionalKinds.JumpListItem;
            }
            else if (!string.IsNullOrEmpty(e?.TileId) && e?.TileId != DefaultTileID)
            {
                return AdditionalKinds.SecondaryTile;
            }
            else
            {
                return AdditionalKinds.Other;
            }
        }

        private object _PageKeys;
        public Dictionary<T, Type> PageKeys<T>()
            where T : struct, IConvertible
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            if (_PageKeys != null && _PageKeys is Dictionary<T, Type>)
            {
                return _PageKeys as Dictionary<T, Type>;
            }
            return (_PageKeys = new Dictionary<T, Type>()) as Dictionary<T, Type>;
        }

        #endregion

        #region Application overides that Template 10 seals

        // it is the intent of Template 10 to no longer require Launched/Activated overrides, only OnStartAsync()

        protected override sealed void OnActivated(IActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(e, StartKind.Activate); }
        protected override sealed void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(e, StartKind.Activate); }
        protected override sealed void OnFileActivated(FileActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(e, StartKind.Activate); }
        protected override sealed void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(e, StartKind.Activate); }
        protected override sealed void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(e, StartKind.Activate); }
        protected override sealed void OnSearchActivated(SearchActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(e, StartKind.Activate); }
        protected override sealed void OnShareTargetActivated(ShareTargetActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(e, StartKind.Activate); }
        protected sealed override void OnLaunched(LaunchActivatedEventArgs e) { DebugWrite(); StartupOrchestratorAsync(e, StartKind.Launch); }

        #endregion

        #region Template 10's new Application overrides

        /// <summary>
        /// If a developer overrides this method, the developer can resolve DataContext or unwrap DataContext 
        /// available for the Page object when using a MVVM pattern that relies on a wrapped/proxy around ViewModels
        /// </summary>
        public virtual INavigable ResolveForPage(Page page, NavigationService navigationService) => null;

        /// <summary>
        /// Prelaunch may never occur. However, it's possible that it will. It is a Windows mechanism
        /// to launch apps in the background and quickly suspend them. Because of this, developers need to
        /// handle Prelaunch scenarios if their typical launch is expensive or requires user interaction.
        /// </summary>
        /// <param name="args">IActivatedEventArgs from startup</param>
        /// <param name="runOnStartAsync">A developer can force the typical startup pipeline. Default should be false.</param>
        /// <remarks>
        /// For Prelaunch Template 10 does not continue the typical startup pipeline by default. 
        /// OnActivated will occur if the application has been prelaunched.
        /// </remarks>
        public virtual Task OnPrelaunchAsync(IActivatedEventArgs args, out bool runOnStartAsync)
        {
            DebugWrite("Virtual");

            runOnStartAsync = false;
            return Task.CompletedTask;
        }

        /// <summary>
        /// OnStartAsync is the one-stop-show override to handle when your app starts
        /// Template 10 will not call OnStartAsync if the app is restored from state.
        /// An app restores from state when the app was suspended and then terminated (PreviousExecutionState terminated).
        /// </summary>
        public abstract Task OnStartAsync(StartKind startKind, IActivatedEventArgs args);

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
        public virtual Task OnSuspendingAsync(object s, SuspendingEventArgs e, bool prelaunchActivated) => Task.CompletedTask;

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

        private async void StartupOrchestratorAsync(IActivatedEventArgs e, StartKind kind)
        {
            DebugWrite($"kind:{kind} previous:{e.PreviousExecutionState}");

            if (OriginalActivatedArgs == null)
            {
                OriginalActivatedArgs = e;

                // if resume tries to launch, don't continue on
                // StartupOrchestratorAsync will be called twice
                if (OriginalActivatedArgs == null)
                {
                    OnResuming(this, null, AppExecutionState.Terminated);
                    return;
                }
            }

            // is the kind really right?
            if (kind == StartKind.Launch && CurrentStateHistory.ContainsValue(BootstrapperStates.Launching))
            {
                kind = StartKind.Activate;
            }
            else if (kind == StartKind.Launch && e.PreviousExecutionState == ApplicationExecutionState.Running)
            {
                kind = StartKind.Activate;
            }
            else if (kind == StartKind.Activate && e.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                kind = StartKind.Launch;
            }

            // handle activate
            if (kind == StartKind.Activate)
            {
                CurrentState = BootstrapperStates.Activating;

                // never activate until launch has completed
                while (!CurrentStateHistory.ContainsValue(BootstrapperStates.Launched))
                {
                    await Task.Delay(10);
                }

                while (CurrentStateHistory.Count(x => x.Value == BootstrapperStates.Starting) != CurrentStateHistory.Count(x => x.Value == BootstrapperStates.Started))
                {
                    await Task.Delay(10);
                }

                CurrentState = BootstrapperStates.Starting;

                await OnStartAsync(kind, e);

                CurrentState = BootstrapperStates.Started;

                WindowLogic.ActivateWindow(ActivateWindowSources.Activating, SplashLogic);

                CurrentState = BootstrapperStates.Activated;
            }

            // handle first-time launch
            else if (kind == StartKind.Launch)
            {
                CurrentState = BootstrapperStates.Launching;

                SplashLogic.Show(e.SplashScreen, this);

                // do some one-time things
                SetupKeyboardListeners();
                SetupLifecycleListeners();
                SetupSystemListeners();
                SetupCustomTitleBar();

                // default Unspecified extended session
                if (AutoExtendExecutionSession)
                {
                    await ExtendedSessionService.StartUnspecifiedAsync();
                }

                CurrentState = BootstrapperStates.Initializing;

                // OnInitializeAsync
                await OnInitializeAsync(e);

                CurrentState = BootstrapperStates.Initialized;

                // if there no pre-existing root then generate root
                if (SplashLogic.Splashing || Window.Current.Content == null)
                {
                    Window.Current.Content = CreateRootElement(e);
                }

                // okay, now handle launch
                bool restored = false;
                var IsPrelaunch = (e as LaunchActivatedEventArgs)?.PrelaunchActivated ?? false;
                switch (e.PreviousExecutionState)
                {
                    case ApplicationExecutionState.Suspended:
                    case ApplicationExecutionState.Terminated:
                        OnResuming(this, null, IsPrelaunch ? AppExecutionState.Prelaunch : AppExecutionState.Terminated);
                        var launchedEventArgs = e as ILaunchActivatedEventArgs;
                        if (AutoRestoreAfterTerminated
                            && DetermineStartCause(e) == AdditionalKinds.Primary || launchedEventArgs?.TileId == string.Empty)
                        {
                            CurrentState = BootstrapperStates.Restoring;

                            restored = await NavigationService.LoadAsync();

                            CurrentState = BootstrapperStates.Restored;
                        }
                        break;
                }

                // handle if pre-launch (no UI)
                if (IsPrelaunch)
                {
                    CurrentState = BootstrapperStates.Prelaunching;

                    var runOnStartAsync = false;
                    await OnPrelaunchAsync(e, out runOnStartAsync);

                    CurrentState = BootstrapperStates.Prelaunched;

                    if (!runOnStartAsync)
                    {
                        return;
                    }
                }

                // handle if not restored (new launch)
                if (!restored)
                {
                    CurrentState = BootstrapperStates.Starting;

                    await OnStartAsync(StartKind.Launch, e);

                    CurrentState = BootstrapperStates.Started;
                }

                // this will also hide any showing splashscreen
                WindowLogic.ActivateWindow(ActivateWindowSources.Launching, SplashLogic);

                CurrentState = BootstrapperStates.Launched;
            }
        }

        async Task TryWrap(BootstrapperStates before, Func<Task> action, BootstrapperStates after)
        {
            CurrentState = before;
            try
            {
                await action();
            }
            finally
            {

            }
        }

        private void SetupSystemListeners()
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) =>
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
            Resuming += (s, e) => StartupOrchestratorAsync(OriginalActivatedArgs, StartKind.Launch);
            Suspending += async (s, e) =>
                {
                    var deferral = e.SuspendingOperation.GetDeferral();
                    try
                    {
                        if (AutoExtendExecutionSession)
                        {
                            // unspecified will be revoked by suspension
                            await ExtendedSessionService.StartSaveDataAsync();
                        }

                        var navs = WindowWrapper.ActiveWrappers.SelectMany(x => x.NavigationServices);
                        foreach (INavigationService nav in navs)
                        {
                            // individual frame-level
                            await nav.SuspendingAsync();
                            if (AutoSuspendAllFrames) await nav.SaveAsync();
                        }

                        // application-level
                        var isPrelaunch = (OriginalActivatedArgs as LaunchActivatedEventArgs)?.PrelaunchActivated ?? false;
                        await OnSuspendingAsync(this, e, isPrelaunch);
                    }
                    finally
                    {
                        ExtendedSessionService.Dispose();
                        deferral.Complete();
                    }
                };
        }

        private void SetupKeyboardListeners()
        {
            var KeyboardService = Services.KeyboardService.KeyboardService.Instance;
            KeyboardService.AfterBackGesture = () =>
            {
                DebugWrite(caller: nameof(KeyboardService.AfterBackGesture));

                var handled = false;
                RaiseBackRequested(ref handled);
            };
            KeyboardService.AfterForwardGesture = () =>
            {
                DebugWrite(caller: nameof(KeyboardService.AfterForwardGesture));

                RaiseForwardRequested();
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
                if (key == typeof(Controls.CustomTitleBar))
                {
                    var style = resource.Value as Style;
                    var title = new Controls.CustomTitleBar();
                    title.Style = style;
                }
                count--;
                if (count == 0) break;
            }
        }
    }
}
