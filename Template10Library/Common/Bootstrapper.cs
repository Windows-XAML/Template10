using System;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Windows.UI.ViewManagement;

namespace Template10.Common
{
    // BootStrapper is a drop-in replacement of Application
    // - OnInitializeAsync is the first in the pipeline, if launching
    // - OnStartAsync is required, and second in the pipeline
    // - NavigationService is an automatic property of this class
    public abstract class BootStrapper : Application
    {
        /// <summary>
        /// Event to allow views and viewmodels to intercept the Hardware/Shell Back request and 
        /// implement their own logic, such as closing a dialog. In your event handler, set the
        /// Handled property of the BackRequestedEventArgs to true if you do not want a Page
        /// Back navigation to occur.
        /// </summary>
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
                    foreach (var service in Windows.SelectMany(x => x.NavigationServices))
                    {
                        // date the cache
                        service.State().Values[CacheKey] = DateTime.Now.ToString();
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

        public Services.NavigationService.NavigationService NavigationService { get { return Windows.First().NavigationServices.First(); } }

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
            Windows.Add(new WindowWrapper(args.Window));
            WindowCreated?.Invoke(this, args);
            base.OnWindowCreated(args);
        }

        public List<WindowWrapper> Windows { get; } = new List<WindowWrapper>();

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

            // setup default view
            var view = Windows.First();

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
            var nav = new Services.NavigationService.NavigationService(frame);
            view.NavigationServices.Add(nav);

            // expire state (based on expiry)
            var state = nav.State();
            if (state.Values.ContainsKey(CacheKey))
            {
                DateTime cacheDate;
                if (DateTime.TryParse(state.Values[CacheKey]?.ToString(), out cacheDate))
                {
                    var cacheAge = DateTime.Now.Subtract(cacheDate);
                    if (cacheAge >= CacheMaxDuration)
                    {
                        foreach (var cache in view.NavigationServices.Select(x => x.State()))
                        {
                            foreach (var container in cache.Containers)
                            {
                                cache.DeleteContainer(container.Key);
                            }
                        }
                    }
                }
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
                        var restored = nav.RestoreSavedNavigation();
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

        public class HandledEventArgs : EventArgs { public System.Boolean Handled { get; set; } }
    }
}
