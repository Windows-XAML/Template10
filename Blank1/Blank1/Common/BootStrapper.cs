using System;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Activation;

namespace Blank1.Common
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
            Resuming += (s, e) =>
            {
                OnResuming(s, e);
            };
            Suspending += async (s, e) =>
            {
                var deferral = e.SuspendingOperation.GetDeferral();
                await NavigationService.SuspendingAsync();
                await OnSuspendingAsync(s, e);
                deferral.Complete();
            };
        }

        #region properties

        public Frame RootFrame { get; set; }
        public Services.NavigationService.NavigationService NavigationService { get; private set; }
        protected Func<SplashScreen, Page> SplashFactory { get; set; }

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

        [Obsolete("Use OnStartAsync()")]
        protected override void OnLaunched(LaunchActivatedEventArgs e) { InternalLaunchAsync(e as ILaunchActivatedEventArgs); }

        private async void InternalLaunchAsync(ILaunchActivatedEventArgs e)
        {
            UIElement splashScreen = default(UIElement);
            if (SplashFactory != null)
            {
                splashScreen = SplashFactory(e.SplashScreen);
                Window.Current.Content = splashScreen;
            }

            RootFrame = RootFrame ?? new Frame();
            RootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
            NavigationService = new Services.NavigationService.NavigationService(RootFrame);

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
                case ApplicationExecutionState.Terminated:
                case ApplicationExecutionState.ClosedByUser:
                    {
                        // restore if you need to/can do
                        var restored = NavigationService.RestoreSavedNavigation();
                        if (!restored)
                            await OnStartAsync(StartKind.Launch, e);
                        break;
                    }
            }

            // if the user didn't already set custom content use rootframe
            if (Window.Current.Content == splashScreen)
            {
                Window.Current.Content = RootFrame;
            }
            if (Window.Current.Content == null)
            {
                Window.Current.Content = RootFrame;
            }

            // ensure active
            Window.Current.Activate();

            // Hook up the default Back handler
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, args) =>
            {
                args.Handled = true;
                OnBackRequested();
            };

            // Hook up keyboard and mouse Back handler
            var keyboard = new Services.KeyboardService.KeyboardService();
            keyboard.AfterBackGesture = () => OnBackRequested();

            // Hook up keyboard and house Forward handler
            keyboard.AfterForwardGesture = () => OnForwardRequested();
        }

        /// <summary>
        /// Default Hardware/Shell Back handler overrides standard Back behavior that navigates to previous app
        /// in the app stack to instead cause a backward page navigation.
        /// Views or Viewodels can override this behavior by handling the BackRequested event and setting the
        /// Handled property of the BackRequestedEventArgs to true.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBackRequested()
        {
            var args = new HandledEventArgs();
            BackRequested?.Invoke(this, args);

            if (!args.Handled)
            {
                NavigationService.GoBack();
                args.Handled = true;
            }
        }

        private void OnForwardRequested()
        {
            var args = new HandledEventArgs();
            ForwardRequested?.Invoke(this, args);

            if (!args.Handled)
            {
                NavigationService.GoForward();
                args.Handled = true;
            }
        }

        #region overrides

        public enum StartKind { Launch, Activate }
        public abstract Task OnStartAsync(StartKind startKind, IActivatedEventArgs args);
        public virtual Task OnInitializeAsync() { return Task.FromResult<object>(null); }
        public virtual Task OnSuspendingAsync(object s, SuspendingEventArgs e) { return Task.FromResult<object>(null); }
        public virtual void OnResuming(object s, object e) { }

        #endregion

        public class HandledEventArgs : EventArgs { public System.Boolean Handled { get; set; } }
    }
}
