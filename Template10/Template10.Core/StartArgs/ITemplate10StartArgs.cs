using Windows.ApplicationModel.Activation;

namespace Template10.StartArgs
{
    public interface ITemplate10StartArgs
    {
        bool ThisIsFirstStart { get; }
        Template10.Template10StartArgs.StartKinds StartKind { get; }
        Template10.Template10StartArgs.StartCauses StartCause { get; }
        ApplicationExecutionState PreviousExecutionState { get; }

        object EventArgs { get; }

        BackgroundActivatedEventArgs BackgroundActivatedEventArgs { get; }
        CachedFileUpdaterActivatedEventArgs CachedFileUpdaterActivatedEventArgs { get; }
        FileActivatedEventArgs FileActivatedEventArgs { get; }
        FileOpenPickerActivatedEventArgs FileOpenPickerActivatedEventArgs { get; }
        FileSavePickerActivatedEventArgs FileSavePickerActivatedEventArgs { get; }
        IActivatedEventArgs IActivatedEventArgs { get; }
        LaunchActivatedEventArgs LaunchActivatedEventArgs { get; }
        SearchActivatedEventArgs SearchActivatedEventArgs { get; }
        ShareTargetActivatedEventArgs ShareTargetActivatedEventArgs { get; }
    }
}