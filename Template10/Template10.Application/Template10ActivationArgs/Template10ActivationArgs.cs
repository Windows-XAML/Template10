using System;
using Windows.ApplicationModel.Activation;

namespace Template10.Common
{
    public class Template10StartArgs
    {
        public Template10StartArgs(object arg, StartKinds startKind)
        {
            Starts++;
            RawEventArgs = arg;
            StartKind = startKind;

            // sometimes launch is actually activate
            if (ThisIsFirstStart && startKind == StartKinds.Activate)
            {
                StartKind = StartKinds.Launch;
            }
            else if (!ThisIsFirstStart && startKind == StartKinds.Launch)
            {
                StartKind = StartKinds.Activate;
            }

            // we need to update PrelaunchOccurred
            if (ThisIsPrelaunch && !PrelaunchOccurred)
            {
                PrelaunchOccurred = true;
            }

            // could PreviousExecutionState be wrong? Yes.
            if (StartKind == StartKinds.Activate)
            {
                PreviousExecutionState = ApplicationExecutionState.Running;
            }
            else if (OnLaunchedEventArgs != null)
            {
                PreviousExecutionState = OnLaunchedEventArgs.PreviousExecutionState;
            }
            else if (OnActivatedEventArgs != null)
            {
                PreviousExecutionState = OnActivatedEventArgs.PreviousExecutionState;
            }
            else
            {
                PreviousExecutionState = ApplicationExecutionState.NotRunning;
            }
        }

        public static int Starts { get; private set; } = 0;
        public object RawEventArgs { get; private set; }
        public StartKinds StartKind { get; private set; }
        public static bool PrelaunchOccurred { get; private set; }
        public bool ThisIsPrelaunch => OnLaunchedEventArgs?.PrelaunchActivated ?? false;
        public ApplicationExecutionState PreviousExecutionState { get; private set; }

        public bool ThisIsFirstStart => Starts.Equals(1);
        public bool ThisIsResuming => Settings.SuspendResumeStrategy.IsResuming(this);
        public StartCauses StartCause => DetermineStartCause(RawEventArgs);

        public IActivatedEventArgs OnActivatedEventArgs => RawEventArgs as IActivatedEventArgs;
        public CachedFileUpdaterActivatedEventArgs OnCachedFileUpdaterActivatedEventArgs => RawEventArgs as CachedFileUpdaterActivatedEventArgs;
        public FileActivatedEventArgs OnFileActivatedEventArgs => RawEventArgs as FileActivatedEventArgs;
        public FileOpenPickerActivatedEventArgs OnFileOpenPickerActivatedEventArgs => RawEventArgs as FileOpenPickerActivatedEventArgs;
        public FileSavePickerActivatedEventArgs OnFileSavePickerActivatedEventArgs => RawEventArgs as FileSavePickerActivatedEventArgs;
        public SearchActivatedEventArgs OnSearchActivatedEventArgs => RawEventArgs as SearchActivatedEventArgs;
        public ShareTargetActivatedEventArgs OnShareTargetActivatedEventArgs => RawEventArgs as ShareTargetActivatedEventArgs;
        public LaunchActivatedEventArgs OnLaunchedEventArgs => RawEventArgs as LaunchActivatedEventArgs;
        public BackgroundActivatedEventArgs OnBackgroundActivatedEventArgs => RawEventArgs as BackgroundActivatedEventArgs;

        public override string ToString()
        {
            return $"kind:{StartKind} cause:{StartCause} previous:{PreviousExecutionState} type:{RawEventArgs?.GetType()}";
        }

        StartCauses DetermineStartCause(object args)
        {
            switch (args)
            {
                case IBackgroundActivatedEventArgs e:
                    return StartCauses.BackgroundTrigger;
                case IToastNotificationActivatedEventArgs e:
                    return StartCauses.Toast;
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
}