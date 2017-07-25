using System;
using Windows.ApplicationModel.Activation;
using static Template10.Template10StartArgs;

namespace Template10.StartArgs
{
    public partial class Template10StartArgs : ITemplate10StartArgs
    {
        internal static int Starts { get; private set; } = 0;

        internal Template10StartArgs(object eventArgs, Template10.Template10StartArgs.StartKinds kind)
        {
            Starts++;
            StartKind = kind;
            EventArgs = eventArgs;

            if (ThisIsFirstStart && StartKind == Template10.Template10StartArgs.StartKinds.Activate)
            {
                StartKind = Template10.Template10StartArgs.StartKinds.Launch;
            }
            else if (!ThisIsFirstStart && StartKind == Template10.Template10StartArgs.StartKinds.Launch)
            {
                StartKind = Template10.Template10StartArgs.StartKinds.Activate;
            }

            if (StartKind == Template10.Template10StartArgs.StartKinds.Activate)
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
        public Template10.Template10StartArgs.StartKinds StartKind { get; }
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

        public StartCauses StartCause
        {
            get
            {
                switch (EventArgs)
                {
                    case IBackgroundActivatedEventArgs e:
                        {
                            return StartCauses.BackgroundTrigger;
                        }
                    case IToastNotificationActivatedEventArgs e:
                        {
                            return StartCauses.Toast;
                        }
                    // TODO for RS3: ICommandLine
                    case ILaunchActivatedEventArgs e when e != null:
                        switch (e.TileId)
                        {
                            case "App" when string.IsNullOrEmpty(e?.Arguments):
                                return StartCauses.Primary;
                            case "App" when !string.IsNullOrEmpty(e?.Arguments):
                                return StartCauses.JumpListItem;
                            case string id when !string.IsNullOrEmpty(id):
                                return StartCauses.SecondaryTile;
                            default:
                                return StartCauses.Other;
                        }
                    default:
                        return StartCauses.Other;
                }
            }
        }

        public override string ToString()
            => $"kind:{StartKind} cause:{StartCause} previous:{PreviousExecutionState} type:{EventArgs?.GetType().ToString() ?? "null"}";
    }
}
