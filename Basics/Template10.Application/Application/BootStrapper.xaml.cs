using System;
using Template10.Application.Services;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Template10.Application
{
    public abstract partial class BootStrapper : Windows.UI.Xaml.Application
    {
#pragma warning disable CS0067 // unused events
        new event EventHandler<object> Resuming;
        private new event SuspendingEventHandler Suspending;
        private new event EnteredBackgroundEventHandler EnteredBackground;
        private new event LeavingBackgroundEventHandler LeavingBackground;
#pragma warning restore CS0067

        protected override sealed async void OnActivated(IActivatedEventArgs e) => await InternalStartAsync(new StartArgs(e), StartKinds.Activate);
        protected override sealed async void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs e) => await InternalStartAsync(new StartArgs(e), StartKinds.Activate);
        protected override sealed async void OnFileActivated(FileActivatedEventArgs e) => await InternalStartAsync(new StartArgs(e), StartKinds.Activate);
        protected override sealed async void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs e) => await InternalStartAsync(new StartArgs(e), StartKinds.Activate);
        protected override sealed async void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs e) => await InternalStartAsync(new StartArgs(e), StartKinds.Activate);
        protected override sealed async void OnSearchActivated(SearchActivatedEventArgs e) => await InternalStartAsync(new StartArgs(e), StartKinds.Activate);
        protected override sealed async void OnShareTargetActivated(ShareTargetActivatedEventArgs e) => await InternalStartAsync(new StartArgs(e), StartKinds.Activate);
        protected override sealed async void OnLaunched(LaunchActivatedEventArgs e) => await InternalStartAsync(new StartArgs(e), StartKinds.Launch);
        protected override sealed async void OnBackgroundActivated(BackgroundActivatedEventArgs e) => await InternalStartAsync(new StartArgs(e), StartKinds.Background);

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            base.OnWindowCreated(args);
            _windowCreated?.Invoke(this, args);
        }
    }
}
