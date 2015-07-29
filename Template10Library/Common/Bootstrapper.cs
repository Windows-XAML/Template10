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
        public event EventHandler<HandledEventArgs> BackRequested;
        public event EventHandler<HandledEventArgs> ForwardRequested;

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
                        service.Frame.SetFrameState(CacheKey, DateTime.Now.ToString());
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

        protected Services.NavigationService.NavigationService NavigationService
        {
            // because it is protected, we can safely assume it will ref the first view
            get { return WindowWrapper.ActiveWrappers.First().NavigationServices.First(); }
        }

        protected Func<SplashScreen, Page> SplashFactory { get; set; }
        public TimeSpan CacheMaxDuration { get; set; } = TimeSpan.MaxValue;
        private const string CacheKey = "Setting-Cache-Date";
        public bool ShowShellBackButton { get; set; } = true;

        #endregion

        #region activated

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

        private async Task InternalActivatedAsync(IActivatedEventArgs e)
        {
            await OnStartAsync(StartKind.Activate, e);
            Window.Current.Activate();
        }

        #endregion

        public event EventHandler<WindowCreatedEventArgs> WindowCreated;
        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            var window = new WindowWrapper(args.Window);
            WindowCreated?.Invoke(this, args);
            base.OnWindowCreated(args);
        }

        [Obsolete("Use OnStartAsync()")]
        protected override void OnLaunched(LaunchActivatedEventArgs e) { InternalLaunchAsync(e as ILaunchActivatedEventArgs); }

        private async void InternalLaunchAsync(ILaunchActivatedEventArgs e)
        {
            // first handle prelaunch, which will not continue
            if ((e.Kind == ActivationKind.Launch) && ((e as LaunchActivatedEventArgs)?.PrelaunchActivated ?? false))
            {
                OnPrelaunch();
                return;
            }

            // now handle normal activation
            UIElement splashScreen = default(UIElement);
            if (SplashFactory != null)
            {
                splashScreen = SplashFactory(e.SplashScreen);
                Window.Current.Content = splashScreen;
            }

            // setup frame
            var frame = new Frame
            {
                Language = global::Windows.Globalization.ApplicationLanguages.Languages[0]
            };
            frame.Navigated += (s, args) =>
            {
                global::Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    (ShowShellBackButton && frame.CanGoBack) ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            };

            // setup default view
            var view = WindowWrapper.ActiveWrappers.First();
            var navigationService = new Services.NavigationService.NavigationService(frame);
            view.NavigationServices.Add(navigationService);

            // expire state (based on expiry)
            DateTime cacheDate;
            var otherwise = DateTime.MinValue.ToString();
            if (DateTime.TryParse(navigationService.Frame.GetFrameState(CacheKey, otherwise), out cacheDate))
            {
                var cacheAge = DateTime.Now.Subtract(cacheDate);
                if (cacheAge >= CacheMaxDuration)
                {
                    // clear state in every nav service in every view
                    foreach (var service in WindowWrapper.ActiveWrappers.SelectMany(x => x.NavigationServices))
                    {
                        service.Frame.ClearFrameState();
                    }
                }
            }
            else
            {
                // no date, that's okay
            }

            // the user may override to set custom content
            await OnInitializeAsync();
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
                        var restored = navigationService.RestoreSavedNavigation();
                        if (!restored)
                        {
                            await OnStartAsync(StartKind.Launch, e);
                        }
                        break;
                    }
            }

            // if the user didn't already set custom content use rootframe
            if (Window.Current.Content == splashScreen) { Window.Current.Content = frame; }
            if (Window.Current.Content == null) { Window.Current.Content = frame; }

            // ensure active
            Window.Current.Activate();

            // Hook up the default Back handler
            global::Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, args) =>
            {
                args.Handled = true;
                RaiseBackRequested();
            };

            // Hook up keyboard and mouse Back handler
            var keyboard = new Services.KeyboardService.KeyboardService();
            keyboard.AfterBackGesture = () => RaiseBackRequested();

            // Hook up keyboard and house Forward handler
            keyboard.AfterForwardGesture = () => RaiseForwardRequested();
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
            BackRequested?.Invoke(this, args);

            if (!args.Handled)
            {
                // default to first window
                NavigationService.GoBack();
                args.Handled = true;
            }
        }

        private void RaiseForwardRequested()
        {
            var args = new HandledEventArgs();
            ForwardRequested?.Invoke(this, args);

            if (!args.Handled)
            {
                // default to first window
                NavigationService.GoForward();
                args.Handled = true;
            }
        }

        #region overrides

        public enum StartKind { Launch, Activate }
        public virtual void OnPrelaunch() { }
        public abstract Task OnStartAsync(StartKind startKind, IActivatedEventArgs args);
        public virtual Task OnInitializeAsync() { return Task.FromResult<object>(null); }
        public virtual Task OnSuspendingAsync(object s, SuspendingEventArgs e) { return Task.FromResult<object>(null); }
        public virtual void OnResuming(object s, object e) { }

        #endregion
    }
}
