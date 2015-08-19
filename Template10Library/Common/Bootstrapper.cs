using System;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using System.Linq;

namespace Template10.Common
{
    public abstract class BootStrapper : Application
    {
        public BootStrapper()
        {
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
        protected Services.NavigationService.NavigationService NavigationService
        {
            // because it is protected, we can safely assume it will ref the first view
            get { return WindowWrapper.ActiveWrappers.First().NavigationServices.First(); }
        }

        /// <summary>
        /// The SplashFactory is a Func<> that returns an instantiated Splash view.
        /// Template 10 will automatically inject this visual before loading the app.
        /// </summary>
        protected Func<SplashScreen, Page> SplashFactory { get; set; }

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

        [Obsolete("Use OnStartAsync()")]
        protected override async void OnActivated(IActivatedEventArgs e) { await InternalActivatedAsync(e); }

        [Obsolete("Use OnStartAsync()")]
        protected override async void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs args) { await InternalActivatedAsync(args); }

        [Obsolete("Use OnStartAsync()")]
        protected override async void OnFileActivated(FileActivatedEventArgs args) { await InternalActivatedAsync(args); }

        [Obsolete("Use OnStartAsync()")]
        protected override async void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args) { await InternalActivatedAsync(args); }

        [Obsolete("Use OnStartAsync()")]
        protected override async void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs args) { await InternalActivatedAsync(args); }

        [Obsolete("Use OnStartAsync()")]
        protected override async void OnSearchActivated(SearchActivatedEventArgs args) { await InternalActivatedAsync(args); }

        [Obsolete("Use OnStartAsync()")]
        protected override async void OnShareTargetActivated(ShareTargetActivatedEventArgs args) { await InternalActivatedAsync(args); }

        // this is private because it is a specialized prelude to OnStartAsync
        private async Task InternalActivatedAsync(IActivatedEventArgs e)
        {
            // sometimes activate requires a frame to be built
            InitializeFrame(e as ILaunchActivatedEventArgs);

            // onstart is shared with activate and launch
            await OnStartAsync(StartKind.Activate, e);

            // ensure active (this will hide any custom splashscreen)
            Window.Current.Activate();
        }

        #endregion

        public event EventHandler<WindowCreatedEventArgs> WindowCreated;

        // It is not possible to hide this override because it is part
        // of the base class. however, should a developer override
        // OnWindowCreated he would also need to call base.OnWindowCreated
        // or the logic in this method, which is critical to Template 10
        // would not be called and cause unstable, unpredictable behavior
        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            var window = new WindowWrapper(args.Window);
            WindowCreated?.Invoke(this, args);
            base.OnWindowCreated(args);
        }

        // it is the intent of Template 10 to no longer require Launched/Activated overrides, only OnStartAsync()

        [Obsolete("Use OnStartAsync()")]
        protected override void OnLaunched(LaunchActivatedEventArgs e) { InternalLaunchAsync(e as ILaunchActivatedEventArgs); }

        // this is private because it is a specialized prelude to OnStartAsync
        private async void InternalLaunchAsync(ILaunchActivatedEventArgs e)
        {
            // the user may override to set custom content
            await OnInitializeAsync(e);

            // create the root frame
            var defaultFrame = InitializeFrame(e);

            // okay, now handle launch
            switch (e.PreviousExecutionState)
            {
                case ApplicationExecutionState.NotRunning:
                case ApplicationExecutionState.Running:
                case ApplicationExecutionState.Suspended:
                    {
                        // launch if not restored
                        await OnStartAsync(StartKind.Launch, e);
                        break;
                    }
                case ApplicationExecutionState.ClosedByUser:
                case ApplicationExecutionState.Terminated:
                    {
                        // restore if you need to/can do
                        // restore if there is no TileId or if the app is luanched via the primary tile
                        // TODO: is it actually possible to have a TileID of null? Don't think so.
                        if (e.TileId == null || e.TileId == "App")
                        {
                            // In the event that the cache ahs expired, attempting to restore
                            // from state will fail because of missing values. This is by design.
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
            }


            // ensure active (this will hide any custom splashscreen)
            Window.Current.Activate();

            // Hook up the default Back handler
            global::Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, args) =>
            {
                // only handle as long as there is a default backstack
                args.Handled = !NavigationService.CanGoBack;
                RaiseBackRequested();
            };

            // Hook up keyboard and mouse Back handler
            var keyboard = new Services.KeyboardService.KeyboardService();
            keyboard.AfterBackGesture = () => RaiseBackRequested();

            // Hook up keyboard and house Forward handler
            keyboard.AfterForwardGesture = () => RaiseForwardRequested();
        }

        // this is private because there's no reason for the developer to call this
        private Frame InitializeFrame(ILaunchActivatedEventArgs e)
        {
            // first show the splash 
            var splashScreen = default(UIElement);
            if (SplashFactory != null)
            {
                splashScreen = SplashFactory(e.SplashScreen);
                Window.Current.Content = splashScreen;
                Window.Current.Activate();
            }
            else
            {
                // if there is no SplashScreen then the default/static splash will show.
            }

            // in any case, next create a new, default frame
            var defaultFrame = (Window.Current.Content as Frame) ?? new Frame
            {
                Language = Windows.Globalization.ApplicationLanguages.Languages[0]
            };

            // set default frame as primary, default visual
            Window.Current.Content = defaultFrame;

            // next setup the default view
            var navigationService = new Services.NavigationService.NavigationService(defaultFrame);
            WindowWrapper.Current().NavigationServices.Add(navigationService);

            // this anon method will be used to refresh the shell back button
            Action updateShellBack = () =>
            {
                // show the shell back only if there is anywhere to go in the default frame
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    (ShowShellBackButton && NavigationService.Frame.CanGoBack)
                        ? AppViewBackButtonVisibility.Visible
                        : AppViewBackButtonVisibility.Collapsed;
            };

            // update shell back when backstack changes
            // only the default frame in this case because secondary should not dismiss the app
            defaultFrame.RegisterPropertyChangedCallback(Frame.BackStackDepthProperty, (s, args) => updateShellBack());

            // update shell back when navigation occurs
            // only the default frame in this case because secondary should not dismiss the app
            defaultFrame.Navigated += (s, args) => updateShellBack();

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

            // finally return our new frame
            return defaultFrame;
        }

        /// <summary>
        /// Default Hardware/Shell Back handler overrides standard Back behavior that navigates to previous app
        /// in the app stack to instead cause a backward page navigation.
        /// Views or Viewodels can override this behavior by handling the BackRequested event and setting the
        /// Handled property of the BackRequestedEventArgs to true.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RaiseBackRequested()
        {
            var args = new HandledEventArgs();
            foreach (var frame in WindowWrapper.Current().NavigationServices.Select(x => x.FrameFacade))
            {
                frame.RaiseBackRequested(args);
                if (args.Handled)
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
        /// </summary>
        /// <param name="startKind"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <remarks>
        /// Template 10 will not call OnStartAsync if the app is restored from state.
        /// An app restores from state when the app was suspended and then terminated (PreviousExecutionState terminated).
        /// </remarks>
        public abstract Task OnStartAsync(StartKind startKind, IActivatedEventArgs args);

        /// <summary>
        /// OnInitializeAsync is where your app will do must-have up-front operations
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// OnInitializeAsync will be called even if the application is restoring from state.
        /// An app restores from state when the app was suspended and then terminated (PreviousExecutionState terminated).
        /// </remarks>
        public virtual Task OnInitializeAsync(IActivatedEventArgs args) { return Task.FromResult<object>(null); }

        /// <summary>
        /// OnSuspendingAsync will be called when the application is suspending, but this override
        /// should only be used by applications that have application-level operations that must
        /// be completed during suspension. 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        /// <remarks>
        /// Using OnSuspendingAsync is a little better than handling the Suspending event manually
        /// because the asunc operations are in a single, global deferral created when the suspension
        /// begins and completed automatically when the last viewmodel has been called (including this method).
        /// </remarks>
        public virtual Task OnSuspendingAsync(object s, SuspendingEventArgs e) { return Task.FromResult<object>(null); }
        public virtual void OnResuming(object s, object e) { }

        #endregion
    }
}
