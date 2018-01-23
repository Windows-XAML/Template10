using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Template10.Application
{
    public abstract partial class Template10Application : Windows.UI.Xaml.Application
    {
#pragma warning disable CS0067 // unused events
        new event EventHandler<object> Resuming;
        private new event SuspendingEventHandler Suspending;
        private new event EnteredBackgroundEventHandler EnteredBackground;
        private new event LeavingBackgroundEventHandler LeavingBackground;
#pragma warning restore CS0067

        protected override sealed async void OnActivated(IActivatedEventArgs e) => await OrchestrateAsync(new StartArgs(e), StartKinds.Activate);
        protected override sealed async void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs e) => await OrchestrateAsync(new StartArgs(e), StartKinds.Activate);
        protected override sealed async void OnFileActivated(FileActivatedEventArgs e) => await OrchestrateAsync(new StartArgs(e), StartKinds.Activate);
        protected override sealed async void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs e) => await OrchestrateAsync(new StartArgs(e), StartKinds.Activate);
        protected override sealed async void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs e) => await OrchestrateAsync(new StartArgs(e), StartKinds.Activate);
        protected override sealed async void OnSearchActivated(SearchActivatedEventArgs e) => await OrchestrateAsync(new StartArgs(e), StartKinds.Activate);
        protected override sealed async void OnShareTargetActivated(ShareTargetActivatedEventArgs e) => await OrchestrateAsync(new StartArgs(e), StartKinds.Activate);
        protected override sealed async void OnLaunched(LaunchActivatedEventArgs e) => await OrchestrateAsync(new StartArgs(e), StartKinds.Launch);
        protected override sealed async void OnBackgroundActivated(BackgroundActivatedEventArgs e) => await OrchestrateAsync(new StartArgs(e), StartKinds.Background);
    }
}
