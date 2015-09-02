using System;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Foundation.Metadata;
using Template10.Services.NavigationService;

namespace Template10.Common
{
    public abstract class BootStrapper : Application
    {
        #region dependency injection

        public virtual T Resolve<T>(Type type) { return default(T); }

        public virtual Services.NavigationService.INavigable ResolveForPage(Type page, NavigationService navigationService) { return null; }

        #endregion

        public static new BootStrapper Current { get; private set; }

        public StateItems SessionState { get; set; } = new StateItems();

        protected BootStrapper()
        {
            Current = this;
            Resuming += (s, e) => { OnResuming(s, e); };
            Suspending += async (s, e) =>
            {
                // one, global deferral
                var deferral = e.SuspendingOperation.GetDeferral();
                try
                {
                    foreach (var service in WindowWrapper.ActiveWrappers.SelectMany(x => x.NavigationServices))
                    {
                        // date the cache (which marks the date/time it was suspended)
                        service.FrameFacade.SetFrameState(CacheDateKey, DateTime.Now.ToString());
                        // call view model suspend (OnNavigatedfrom)
                        await service.SuspendingAsync();
                    }
                    // call system-level suspend
                    await OnSuspendingAsync(s, e);
                }
                catch { }
                finally { deferral.Complete(); }
            };
        }

        #region properties

        /// <summary>
        /// This is the NavigationService of the primary, first, or default Frame. 
        /// An app can contain many frames and reference to this NavigationService should
        /// not be confused as the NavigationService to the "current" Frame as
        /// it is only a helper property to provide the NavigatioNService for
        /// the first Frame ultimately aggregated in the static WindowWrapper class.
        /// </summary>
        public Services.NavigationService.NavigationService NavigationService
        {
            // because it is protected, we can safely assume it will ref the first view
            get { return WindowWrapper.ActiveWrappers.First().NavigationServices.First(); }
        }

        /// <summary>
        /// The SplashFactory is a Func<> that returns an instantiated Splash view.
        /// Template 10 will automatically inject this visual before loading the app.
        /// </summary>
        protected Func<SplashScreen, UserControl> SplashFactory { get; set; }

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

        #endregion

        #region activated

        // it is the intent of Template 10 to no longer require Launched/Activated overrides, only OnStartAsync()

#pragma warning disable 809

        [Obsolete("Use OnStartAsync()")]
        protected override sealed async void OnActivated(IActivatedEventArgs e) { await InternalActivatedAsync(e); }

        [Obsolete("Use OnStartAsync()")]
        protected override sealed async void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs args) { await InternalActivatedAsync(args); }

        [Obsolete("Use OnStartAsync()")]
        protected override sealed async void OnFileActivated(FileActivatedEventArgs args) { await InternalActivatedAsync(args); }

        [Obsolete("Use OnStartAsync()")]
        protected override sealed async void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args) { await InternalActivatedAsync(args); }

        [Obsolete("Use OnStartAsync()")]
        protected override sealed async void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs args) { await InternalActivatedAsync(args); }

        [Obsolete("Use OnStartAsync()")]
        protected override sealed async void OnSearchActivated(SearchActivatedEventArgs args) { await InternalActivatedAsync(args); }

        [Obsolete("Use OnStartAsync()")]
        protected override sealed async void OnShareTargetActivated(ShareTargetActivatedEventArgs args) { await InternalActivatedAsync(args); }

#pragma warning restore 809

        /// <summary>
        /// This handles all the prelimimary stuff unique to Activated before calling OnStartAsync()
        /// This is private because it is a specialized prelude to OnStartAsync().
        /// OnStartAsync will not be called if state restore is determined.
        /// </summary>
        private async Task InternalActivatedAsync(IActivatedEventArgs e)
        {
            // sometimes activate requires a frame to be built
            if (Window.Current.Content == null)
            {
                await InitializeFrameAsync(e as ILaunchActivatedEventArgs);
            }

            // onstart is shared with activate and launch
            await OnStartAsync(StartKind.Activate, e);

            // ensure active (this will hide any custom splashscreen)
            Window.Current.Activate();
        }

        #endregion

        public event EventHandler<WindowCreatedEventArgs> WindowCreated;

        /// <summary>
        /// It is not possible to hide this override because it is part
        /// of the base class. however, should a developer override
        /// OnWindowCreated he would also need to call base.OnWindowCreated
        /// or the logic in this method, which is critical to Template 10
        /// would not be called and cause unstable, unpredictable behavior.
        /// If you need to handle this overload, use the alternative 
        /// WindowCreated event that will be raised by this implementation.
        /// </summary>
        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            var window = new WindowWrapper(args.Window);
            WindowCreated?.Invoke(this, args);
            base.OnWindowCreated(args);
        }

        #region launch

        // it is the intent of Template 10 to no longer require Launched/Activated overrides, only OnStartAsync()

#pragma warning disable 809

        [Obsolete("Use OnStartAsync()")]
        protected override void OnLaunched(LaunchActivatedEventArgs e) { InternalLaunchAsync(e as ILaunchActivatedEventArgs); }

#pragma warning restore 809

        /// <summary>
        /// This handles all the preliminary stuff unique to Launched before calling OnStartAsync().
        /// This is private because it is a specialized prelude to OnStartAsync().
        /// OnStartAsync will not be called if state restore is determined
        /// </summary>
        private async void InternalLaunchAsync(ILaunchActivatedEventArgs e)
        {
            await InitializeFrameAsync(e);

            // okay, now handle launch
            switch (e.PreviousExecutionState)
            {
                case ApplicationExecutionState.ClosedByUser:
                case ApplicationExecutionState.Terminated:
                    {
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
                            var restored = NavigationService.RestoreSavedNavigation();
                            if (!restored)
                            {
                                await OnStartAsync(StartKind.Launch, e);
                            }
                        }
                        else
                        {
                            await OnStartAsync(StartKind.Launch, e);
                        }
                        break;
                    }
                default:
                    {
                        // launch if not restored
                        await OnStartAsync(StartKind.Launch, e);
                        break;
                    }
            }


            // ensure active (this will hide any custom splashscreen)
            Window.Current.Activate();

            // Hook up the default Back handler
            global::Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, args) =>
            {
                // only handle as long as there is a default backstack
                //args.Handled = !NavigationService.CanGoBack;
                var handled = false;
                RaiseBackRequested(ref handled);
                args.Handled = handled;
            };

            // Hook up keyboard and mouse Back handler
            var keyboard = new Services.KeyboardService.KeyboardService();
            keyboard.AfterBackGesture = () =>
                                        {
                                            //the result is no matter
                                            var handled = false;
                                            RaiseBackRequested(ref handled);
                                        };

            // Hook up keyboard and mouse Forward handler
            keyboard.AfterForwardGesture = () => RaiseForwardRequested();

            // Handle the back button press on Mobile
            if (ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0))
            {  
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += (s, a) =>
                {
                    bool handled = false;
                    RaiseBackRequested(ref handled);
                    a.Handled = handled;
                };
            }
        }

        #endregion

        /// <summary>
        /// Default Hardware/Shell Back handler overrides standard Back behavior 
        /// that navigates to previous app in the app stack to instead cause a backward page navigation.
        /// Views or Viewodels can override this behavior by handling the BackRequested 
        /// event and setting the Handled property of the BackRequestedEventArgs to true.
        /// </summary>
        private void RaiseBackRequested(ref bool handled)
        {
            var args = new HandledEventArgs();
            foreach (var frame in WindowWrapper.Current().NavigationServices.Select(x => x.FrameFacade))
            {
                frame.RaiseBackRequested(args);
                handled = args.Handled;
                if (handled)
                    return;
            }

            // default to first window
            NavigationService.GoBack();
        }

        private void RaiseForwardRequested()
        {
            var args = new HandledEventArgs();
            foreach (var frame in WindowWrapper.Current().NavigationServices.Select(x => x.FrameFacade))
            {
                frame.RaiseForwardRequested(args);
                if (args.Handled)
                    return;
            }

            // default to first window
            NavigationService.GoForward();
        }

        #region overrides

        public enum StartKind { Launch, Activate }

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
        public virtual Task OnInitializeAsync(IActivatedEventArgs args) { return Task.FromResult<object>(null); }

        /// <summary>
        /// OnSuspendingAsync will be called when the application is suspending, but this override
        /// should only be used by applications that have application-level operations that must
        /// be completed during suspension. 
        /// Using OnSuspendingAsync is a little better than handling the Suspending event manually
        /// because the asunc operations are in a single, global deferral created when the suspension
        /// begins and completed automatically when the last viewmodel has been called (including this method).
        /// </summary>
        public virtual Task OnSuspendingAsync(object s, SuspendingEventArgs e) { return Task.FromResult<object>(null); }
        public virtual void OnResuming(object s, object e) { }

        #endregion

        /// <summary>
        /// InitializeFrameAsync creates a default Frame preceeded by the optional 
        /// splash screen, then OnInitialzieAsync, then the new frame (if necessary).
        /// This is private because there's no reason for the developer to call this.
        /// </summary>
        private async Task InitializeFrameAsync(ILaunchActivatedEventArgs e)
        {
            // first show the splash 
            FrameworkElement splash = null;
            if (SplashFactory != null)
            {
                Window.Current.Content = splash = SplashFactory(e.SplashScreen);
                Window.Current.Activate();
            }

            // allow the user to do things, even when restoring
            await OnInitializeAsync(e);

            // create the default frame only if there's nothing already there
            // if it is not null, by the way, then the developer injected something & they win
            if (Window.Current.Content == null || Window.Current.Content == splash)
            {
                // build the default frame
                Window.Current.Content = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include).Frame;
            }
        }

        public enum BackButton { Attach, Ignore }
        public enum ExistingContent { Include, Exclude }

        /// <summary>
        /// Craetes a new FamFrame and adds the resulting NavigationService to the 
        /// WindowWrapper collection. In addition, it optionally will setup the 
        /// shell back button to react to the nav of the Frame.
        /// A developer should call this when creating a new/secondary frame.
        /// The shell back button should only be setup one time.
        /// </summary>
        public Services.NavigationService.NavigationService NavigationServiceFactory(BackButton backButton, ExistingContent existingContent)
        {
            var frame = new Frame
            {
                Language = Windows.Globalization.ApplicationLanguages.Languages[0],
                Content = (existingContent == ExistingContent.Include) ? Window.Current.Content : null,
            };

            var navigationService = new Services.NavigationService.NavigationService(frame);
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
            if (DateTime.TryParse(navigationService.FrameFacade.GetFrameState(CacheDateKey, otherwise), out cacheDate))
            {
                var cacheAge = DateTime.Now.Subtract(cacheDate);
                if (cacheAge >= CacheMaxDuration)
                {
                    // clear state in every nav service in every view
                    foreach (var service in WindowWrapper.ActiveWrappers.SelectMany(x => x.NavigationServices))
                    {
                        service.FrameFacade.ClearFrameState();
                    }
                }
            }
            else
            {
                // no date, that's okay
            }

            return navigationService;
        }

        public const string DefaultTileID = "App";
        public void UpdateShellBackButton()
        {
            // show the shell back only if there is anywhere to go in the default frame
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                (ShowShellBackButton && NavigationService.Frame.CanGoBack)
                    ? AppViewBackButtonVisibility.Visible
                    : AppViewBackButtonVisibility.Collapsed;
        }

        public enum AdditionalKinds { Primary, Toast, SecondaryTile, Other }

        /// <summary>
        /// This determines the simplest case for starting. This should handle 80% of common scenarios. 
        /// When Other is returned the developer must determine start manually using IActivatedEventArgs.Kind
        /// </summary>
        public static AdditionalKinds DetermineStartCause(IActivatedEventArgs args)
        {
            var e = args as ILaunchActivatedEventArgs;
            if (e?.TileId == DefaultTileID && string.IsNullOrEmpty(e?.Arguments))
                return AdditionalKinds.Primary;
            else if (e?.TileId == DefaultTileID && !string.IsNullOrEmpty(e?.Arguments))
                return AdditionalKinds.Toast;
            else if (e?.TileId != null && e?.TileId != DefaultTileID)
                return AdditionalKinds.SecondaryTile;
            else
                return AdditionalKinds.Other;
        }
    }
}
