using System;
using System.Linq;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Activation;

namespace Template10.Common
{
    // BootStrapper is a drop-in replacement of Application
    // - OnInitializeAsync is the first in the pipeline, if launching
    // - OnLaunchedAsync is required, and second in the pipeline
    // - OnActivatedAsync is first in the pipeline, if activating
    // - NavigationService is an automatic property of this class
    public abstract class BootStrapper : Application
    {
        public BootStrapper()
        {
            this.Resuming += (s, e) =>
            {
                OnResuming(s, e);
            };
            this.Suspending += async (s, e) =>
            {
                var deferral = e.SuspendingOperation.GetDeferral();
                this.NavigationService.Suspending();
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

        protected override async void OnActivated(IActivatedEventArgs e) { await InternalActivatedAsync(e); }
        protected override async void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs args) { await InternalActivatedAsync(args); }
        protected override async void OnFileActivated(FileActivatedEventArgs args) { await InternalActivatedAsync(args); }
        protected override async void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args) { await InternalActivatedAsync(args); }
        protected override async void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs args) { await InternalActivatedAsync(args); }
        protected override async void OnSearchActivated(SearchActivatedEventArgs args) { await InternalActivatedAsync(args); }
        protected override async void OnShareTargetActivated(ShareTargetActivatedEventArgs args) { await InternalActivatedAsync(args); }

        private async Task InternalActivatedAsync(IActivatedEventArgs e)
        {
            await this.OnActivatedAsync(e);
            Window.Current.Activate();
        }

        #endregion

        protected override void OnLaunched(LaunchActivatedEventArgs e) { InternalLaunchAsync(e as ILaunchActivatedEventArgs); }

        private async void InternalLaunchAsync(ILaunchActivatedEventArgs e)
        {
            if (this.SplashFactory != null)
            {
                Window.Current.Content = this.SplashFactory(e.SplashScreen);
                Window.Current.Activate();
                Window.Current.Content = null;
            }

            this.RootFrame = this.RootFrame ?? new Frame();
            this.RootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
            this.NavigationService = new Services.NavigationService.NavigationService(this.RootFrame);

            // the user may override to set custom content
            await OnInitializeAsync();

            // if the user didn't set custom content, use frame
            if (Window.Current.Content == null)
            { Window.Current.Content = this.RootFrame; }

            if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                try { /* TODO: restore state */ }
                catch { /* TODO: handle fail */ }
            }
            else
            {
                // this is to handle any other type of launch
                await this.OnLaunchedAsync(e);
            }
            Window.Current.Activate();
        }

        #region overrides

        public virtual Task OnInitializeAsync() { return Task.FromResult<object>(null); }
        public virtual Task OnActivatedAsync(IActivatedEventArgs e) { return Task.FromResult<object>(null); }
        public abstract Task OnLaunchedAsync(ILaunchActivatedEventArgs e);
        protected virtual void OnResuming(object s, object e) { }
        protected virtual Task OnSuspendingAsync(object s, SuspendingEventArgs e) { return Task.FromResult<object>(null); }

        #endregion
    }
}
