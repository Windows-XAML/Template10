using System;
using Windows.ApplicationModel.Activation;

namespace Template10.StartArgs
{
    public partial class Template10StartArgs : ITemplate10StartArgs
    {
        public static ITemplate10StartArgs Create(object eventArgs, StartKinds kind)
          => new Template10StartArgs(eventArgs, kind);

        internal static int Starts { get; private set; } = 0;

        private Template10StartArgs(object eventArgs, StartKinds kind)
        {
            Starts++;
            StartKind = kind;
            EventArgs = eventArgs;

            if (ThisIsFirstStart && StartKind == StartKinds.Activate)
            {
                StartKind = StartKinds.Launch;
            }
            else if (!ThisIsFirstStart && StartKind == StartKinds.Launch)
            {
                StartKind = StartKinds.Activate;
            }

            if (StartKind == StartKinds.Activate)
            {
                PreviousExecutionState = ApplicationExecutionState.Running;
            }
            else if (LaunchActivatedEventArgs != null)
            {
                PreviousExecutionState = LaunchActivatedEventArgs.PreviousExecutionState;
            }
            else if (LaunchActivatedEventArgs != null)
            {
                PreviousExecutionState = LaunchActivatedEventArgs.PreviousExecutionState;
            }
            else
            {
                PreviousExecutionState = ApplicationExecutionState.NotRunning;
            }
        }

        public object EventArgs { get; }
        public StartKinds StartKind { get; }
        public bool ThisIsFirstStart => Starts.Equals(1);
        public ApplicationExecutionState PreviousExecutionState { get; }

        public IActivatedEventArgs IActivatedEventArgs => EventArgs as IActivatedEventArgs;
        public CachedFileUpdaterActivatedEventArgs CachedFileUpdaterActivatedEventArgs => EventArgs as CachedFileUpdaterActivatedEventArgs;
        public FileActivatedEventArgs FileActivatedEventArgs => EventArgs as FileActivatedEventArgs;
        public FileOpenPickerActivatedEventArgs FileOpenPickerActivatedEventArgs => EventArgs as FileOpenPickerActivatedEventArgs;
        public FileSavePickerActivatedEventArgs FileSavePickerActivatedEventArgs => EventArgs as FileSavePickerActivatedEventArgs;
        public SearchActivatedEventArgs SearchActivatedEventArgs => EventArgs as SearchActivatedEventArgs;
        public ShareTargetActivatedEventArgs ShareTargetActivatedEventArgs => EventArgs as ShareTargetActivatedEventArgs;
        public LaunchActivatedEventArgs LaunchActivatedEventArgs => EventArgs as LaunchActivatedEventArgs;
        public BackgroundActivatedEventArgs BackgroundActivatedEventArgs => EventArgs as BackgroundActivatedEventArgs;

        public Template10StartArgs.StartCauses StartCause
        {
            get
            {
                switch (EventArgs)
                {
                    case IBackgroundActivatedEventArgs e:
                        {
                            return Template10StartArgs.StartCauses.BackgroundTrigger;
                        }
                    case IToastNotificationActivatedEventArgs e:
                        {
                            return Template10StartArgs.StartCauses.Toast;
                        }
                    // TODO for RS3: ICommandLine
                    case ILaunchActivatedEventArgs e when e != null:
                        switch (e.TileId)
                        {
                            case "App" when string.IsNullOrEmpty(e?.Arguments):
                                return Template10StartArgs.StartCauses.Primary;
                            case "App" when !string.IsNullOrEmpty(e?.Arguments):
                                return Template10StartArgs.StartCauses.JumpListItem;
                            case string id when !string.IsNullOrEmpty(id):
                                return Template10StartArgs.StartCauses.SecondaryTile;
                            default:
                                return Template10StartArgs.StartCauses.Undetermined;
                        }
                    default:
                        return Template10StartArgs.StartCauses.Undetermined;
                }
            }
        }

        public override string ToString()
            => $"kind:{StartKind} cause:{StartCause} previous:{PreviousExecutionState} type:{EventArgs?.GetType().ToString() ?? "null"}";
    }
}
