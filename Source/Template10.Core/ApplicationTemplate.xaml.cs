using Template10.Core.Services;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Template10
{
    public abstract partial class ApplicationTemplate : Application
    {
        protected sealed override async void OnActivated(IActivatedEventArgs e) => await InternalStartAsync(new StartArgs(e, StartKinds.Activate));
        protected sealed override async void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs e) => await InternalStartAsync(new StartArgs(e, StartKinds.Activate));
        protected sealed override async void OnFileActivated(FileActivatedEventArgs e) => await InternalStartAsync(new StartArgs(e, StartKinds.Activate));
        protected sealed override async void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs e) => await InternalStartAsync(new StartArgs(e, StartKinds.Activate));
        protected sealed override async void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs e) => await InternalStartAsync(new StartArgs(e, StartKinds.Activate));
        protected sealed override async void OnSearchActivated(SearchActivatedEventArgs e) => await InternalStartAsync(new StartArgs(e, StartKinds.Activate));
        protected sealed override async void OnShareTargetActivated(ShareTargetActivatedEventArgs e) => await InternalStartAsync(new StartArgs(e, StartKinds.Activate));
        protected sealed override async void OnLaunched(LaunchActivatedEventArgs e) => await InternalStartAsync(new StartArgs(e, StartKinds.Launch));
        protected sealed override async void OnBackgroundActivated(BackgroundActivatedEventArgs e) => await InternalStartAsync(new StartArgs(e, StartKinds.Background));

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            base.OnWindowCreated(args);
            WindowService.ForwardWindowCreated(args);
        }
    }
}
