using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Controls;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Common
{
    public abstract partial class BootStrapper : Application, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Set<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (!object.Equals(storage, value))
            {
                storage = value;
                RaisePropertyChanged(propertyName);
            }
        }

        protected void RaisePropertyChanged([CallerMemberName] String propertyName = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        public StateItems SessionState { get; set; } = new StateItems();

        #region Debug

        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = Services.LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            Services.LoggingService.LoggingService.WriteLine(text, severity, caller: $"BootStrapper.{caller}");

        #endregion

        private void Loaded()
        {
            DebugWrite();
            Current = this;

            // Hook up keyboard and mouse Back handler
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

            // Hook up the default Back handler
            SystemNavigationManager.GetForCurrentView().BackRequested += BackHandler;

            Resuming += HandleResuming;
            Suspending += HandleSuspending;
        }

        public event EventHandler<WindowCreatedEventArgs> WindowCreated;
        protected sealed override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            DebugWrite();

            if (!WindowWrapper.ActiveWrappers.Any())
                Loaded();

            // handle window
            var window = new WindowWrapper(args.Window);
            WindowCreated?.Invoke(this, args);
            base.OnWindowCreated(args);
        }

        #region properties

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

        #endregion

        public IActivatedEventArgs OriginalActivatedArgs { get; private set; }

        /// <summary>
        /// This handles all the prelimimary stuff unique to Activated before calling OnStartAsync()
        /// This is private because it is a specialized prelude to OnStartAsync().
        /// OnStartAsync will not be called if state restore is determined.
        /// </summary>
        private async Task InternalActivatedAsync(IActivatedEventArgs e)
        {
            DebugWrite();

            OriginalActivatedArgs = e;

            // sometimes activate requires a frame to be built
            if (Window.Current.Content == null)
            {
                DebugWrite("Calling", caller: nameof(InternalActivatedAsync));
                await InitializeFrameAsync(e);
            }

            // onstart is shared with activate and launch
            await CallOnStartAsync(true, StartKind.Activate);

            // ensure active (this will hide any custom splashscreen)
            ActivateWindow(ActivateWindowSources.Activating);
        }

        /// <summary>
        /// This handles all the preliminary stuff unique to Launched before calling OnStartAsync().
        /// This is private because it is a specialized prelude to OnStartAsync().
        /// OnStartAsync will not be called if state restore is determined
        /// </summary>
        private async void InternalLaunchAsync(ILaunchActivatedEventArgs e)
        {
            DebugWrite($"Previous:{e.PreviousExecutionState.ToString()}");

            OriginalActivatedArgs = e;

            if (e.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                try
                {
                    await InitializeFrameAsync(e);
                }
                catch (Exception)
                {
                    // nothing
                }
            }

            // okay, now handle launch
            bool restored = false;
            switch (e.PreviousExecutionState)
            {
                case ApplicationExecutionState.Suspended:
                case ApplicationExecutionState.Terminated:
                    {
                        OnResuming(this, null, AppExecutionState.Terminated);

                        /*
                            Restore state if you need to/can do.
                            Remember that only the primary tile should restore.
                            (this includes toast with no data payload)
                            The rest are already providing a nav path.

                            In the event that the cache has expired, attempting to restore
                            from state will fail because of missing values. 
                            This is okay & by design.
                        */

                        if (DetermineStartCause(e) == AdditionalKinds.Primary)
                        {
                            restored = await NavigationService.RestoreSavedNavigationAsync();
                            DebugWrite($"{nameof(restored)}:{restored}", caller: nameof(NavigationService.RestoreSavedNavigationAsync));
                        }
                        break;
                    }
                case ApplicationExecutionState.ClosedByUser:
                case ApplicationExecutionState.NotRunning:
                default:
                    break;
            }

            // handle pre-launch
            await CallOnPrelaunchAsync(e);

            if (!restored)
            {
                await CallOnStartAsync(true, StartKind.Launch);
            }

            // ensure active (this will hide any custom splashscreen)
            ActivateWindow(ActivateWindowSources.Launching);
        }

        private void BackHandler(object sender, BackRequestedEventArgs args)
        {
            DebugWrite();

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
            args.Handled = handled;
        }

        
        private void RaiseBackRequested(ref bool handled)
        {
            /*
            Default Hardware/Shell Back handler overrides standard Back behavior 
            that navigates to previous app in the app stack to instead cause a backward page navigation.
            Views or Viewodels can override this behavior by handling the BackRequested 
            event and setting the Handled property of the BackRequestedEventArgs to true.
            */

            DebugWrite();

            var args = new HandledEventArgs();
            BackRequested?.Invoke(null, args);
            if (handled = args.Handled)
                return;
            foreach (var frame in WindowWrapper.Current().NavigationServices.Select(x => x.FrameFacade).Reverse())
            {
                frame.RaiseBackRequested(args);
                if (handled = args.Handled)
                    return;
            }
            NavigationService.GoBack();
        }

        private void RaiseForwardRequested()
        {
            DebugWrite();

            var args = new HandledEventArgs();
            ForwardRequested?.Invoke(null, args);
            if (args.Handled)
                return;
            foreach (var frame in WindowWrapper.Current().NavigationServices.Select(x => x.FrameFacade))
            {
                frame.RaiseForwardRequested(args);
                if (args.Handled)
                    return;
            }
            NavigationService.GoForward();
        }

        #region overrides

        public enum StartKind { Launch, Activate }

        /// <summary>
        /// This indicates if OnPreLaunch has EVER been called.
        /// </summary>
        bool _HasOnPrelaunchAsync = false;

        /// <summary>
        /// This indicates if OnStartAsync has EVER been called
        /// </summary>
        bool _HasOnStartAsync = false;

        /// <summary>
        /// This indicates if OnInitAsync has EVER been called
        /// </summary>
        bool _HasOnInitializeAsync = false;

        public enum AppExecutionState { Suspended, Terminated, Prelaunch }

        #endregion

        private INavigationService DefaultNavigationServiceFactory(BackButton backButton, ExistingContent existingContent, Frame frame)
        {
            DebugWrite($"{nameof(backButton)}:{backButton} {nameof(existingContent)}:{existingContent} {nameof(frame)}:{frame}");

            frame.Content = (existingContent == ExistingContent.Include) ? Window.Current.Content : null;

            // if the service already exists for this frame, use the existing one.
            var existing = frame.GetNavigationService();
            if (existing != null)
            {
                return existing;
            }
            // setup new frame, and include it
            var service = CreateNavigationService(frame);
            service.FrameFacade.BackButtonHandling = backButton;
            WindowWrapper.Current().NavigationServices.Add(service);

            AttachBackButton(backButton, frame);

            ClearExpiredCache(service);

            return service;
        }

        #region pipeline

        public enum States { Starting, Splashing, ShowingContent }
        public States CurrentState { get; set; } = States.Starting;

        private async Task InitializeFrameAsync(IActivatedEventArgs e)
        {
            /*
            InitializeFrameAsync creates a default Frame preceeded by the optional 
            splash screen, then OnInitialzieAsync, then the new frame (if necessary).
            This is private because there's no reason for the developer to call this.
            */

            DebugWrite($"{nameof(IActivatedEventArgs)}:{e.Kind}");

            CreateSplashScreen(e);

            await CallOnInitializeAsync(true, e);

            CustomTitleBar.Setup();

            if (CurrentState == States.Splashing)
            {
                Window.Current.Content = CreateRootElement(e);
            }
            else if (Window.Current.Content == null)
            {
                Window.Current.Content = CreateRootElement(e);
            }
            else
            {
                // if there's custom content then there's nothing to do
            }
        }

        #endregion

        #region Workers

        private void ClearExpiredCache(INavigationService service)
        {
            // this is always okay to check, default or not; expire any state (based on expiry)
            DateTime cacheDate;

            // default the cache age to very fresh if not known
            var otherwise = DateTime.MinValue.ToString();
            if (DateTime.TryParse(service.FrameFacade.GetFrameState(CacheDateKey, otherwise), out cacheDate))
            {
                var cacheAge = DateTime.Now.Subtract(cacheDate);
                if (cacheAge >= CacheMaxDuration)
                {
                    // clear state in every nav service in every view
                    foreach (var nav in WindowWrapper.ActiveWrappers.SelectMany(x => x.NavigationServices))
                    {
                        nav.FrameFacade.ClearFrameState();
                    }
                }
            }
            else
            {
                // no date, that's okay
            }
        }

        private void AttachBackButton(BackButton backButton, Frame frame)
        {
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
        }

        private async Task CallOnPrelaunchAsync(ILaunchActivatedEventArgs e)
        {
            if ((e as LaunchActivatedEventArgs)?.PrelaunchActivated ?? false)
            {
                var runOnStartAsync = false;
                _HasOnPrelaunchAsync = true;
                await OnPrelaunchAsync(e, out runOnStartAsync);
                if (!runOnStartAsync)
                    return;
            }
        }

        private async Task CallOnInitializeAsync(bool canRepeat, IActivatedEventArgs e)
        {
            if (!canRepeat && _HasOnInitializeAsync)
                return;
            _HasOnInitializeAsync = true;
            await OnInitializeAsync(e);
        }

        private async Task CallOnStartAsync(bool canRepeat, StartKind startKind)
        {
            DebugWrite();

            if (!canRepeat && _HasOnStartAsync)
                return;
            _HasOnStartAsync = true;
            await OnStartAsync(startKind, OriginalActivatedArgs);
        }

        private FrameworkElement CreateSplashScreen(IActivatedEventArgs e)
        {
            DebugWrite();

            FrameworkElement splash = null;
            if (SplashFactory != null && e.PreviousExecutionState != ApplicationExecutionState.Suspended)
            {
                Window.Current.Content = splash = SplashFactory(e.SplashScreen);
                CurrentState = States.Splashing;
                ActivateWindow(ActivateWindowSources.SplashScreen);
            }
            return splash;
        }

        private async void HandleResuming(object sender, object e)
        {
            DebugWrite();

            if ((OriginalActivatedArgs as LaunchActivatedEventArgs)?.PrelaunchActivated ?? true)
            {
                OnResuming(sender, e, AppExecutionState.Prelaunch);
                await CallOnStartAsync(false, StartKind.Launch);
                ActivateWindow(ActivateWindowSources.Resuming);
            }
            else
                OnResuming(sender, e, AppExecutionState.Suspended);
        }

        private async void HandleSuspending(object sender, SuspendingEventArgs e)
        {
            DebugWrite();

            // one, global deferral
            var deferral = e.SuspendingOperation.GetDeferral();
            using (var session = new Windows.ApplicationModel.ExtendedExecution.ExtendedExecutionSession
            {
                Description = this.GetType().ToString(),
                Reason = Windows.ApplicationModel.ExtendedExecution.ExtendedExecutionReason.SavingData
            })
            {
                try
                {
                    var services = WindowWrapper.ActiveWrappers.SelectMany(x => x.NavigationServices);
                    foreach (INavigationService nav in services)
                    {
                        // date the cache (which marks the date/time it was suspended)
                        nav.FrameFacade.SetFrameState(CacheDateKey, DateTime.Now.ToString());
                        // call view model suspend (OnNavigatedfrom)
                        DebugWrite($"Nav:{nav}", caller: nameof(nav.SuspendingAsync));
                        await (nav as INavigationService).Dispatcher.DispatchAsync(async () => await nav.SuspendingAsync());
                    }

                    // call system-level suspend
                    DebugWrite($"Calling. Prelaunch {(OriginalActivatedArgs as LaunchActivatedEventArgs)?.PrelaunchActivated ?? false}", caller: nameof(OnSuspendingAsync));
                    await OnSuspendingAsync(sender, e, (OriginalActivatedArgs as LaunchActivatedEventArgs)?.PrelaunchActivated ?? false);
                }
                catch { /* do nothing */ }
                finally { deferral.Complete(); }
            }
        }

        #endregion

        // The default frame is automatically wrapped in a modal dialog.
        // this is how you access it to set ModalContent or the IsModal property. 
        public Controls.ModalDialog ModalDialog { get { return (Window.Current.Content as Controls.ModalDialog); } }
        public UIElement ModalContent { get { return ModalDialog?.ModalContent; } set { if (ModalDialog != null) ModalDialog.ModalContent = value; } }

        public enum BackButton { Attach, Ignore }

        public enum ExistingContent { Include, Exclude }

        public const string DefaultTileID = "App";

        public enum AdditionalKinds { Primary, Toast, SecondaryTile, Other, JumpListItem }

        private object _PageKeys;
        // T must be a custom Enum
        public Dictionary<T, Type> PageKeys<T>()
            where T : struct, IConvertible
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
                throw new ArgumentException("T must be an enumerated type");
            if (_PageKeys != null && _PageKeys is Dictionary<T, Type>)
                return _PageKeys as Dictionary<T, Type>;
            return (_PageKeys = new Dictionary<T, Type>()) as Dictionary<T, Type>;
        }
    }
}
