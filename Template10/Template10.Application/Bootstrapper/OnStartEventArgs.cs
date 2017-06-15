using Windows.ApplicationModel.Activation;

namespace Template10.Common
{
    public class OnStartEventArgs
    {
        public OnStartEventArgs(object arg, StartKinds startKind)
        {
            RawEventArgs = arg;
            StartKind = startKind;

            // sometimes it's not right
            if (Starts == 1 && startKind == StartKinds.Activate)
            {
                StartKind = StartKinds.Launch;
            }
            else if (Starts > 1 && startKind == StartKinds.Launch)
            {
                StartKind = StartKinds.Activate;
            }
            StartCause = DetermineStartCause(arg);

            Starts++;

            ThisIsPrelaunch = OnLaunchedEventArgs?.PrelaunchActivated ?? false;
            if (!PrelaunchOccurred && ThisIsPrelaunch)
            {
                PrelaunchOccurred = true;
            }

            StartCauses DetermineStartCause(object args)
            {
                if (args is ToastNotificationActivatedEventArgs)
                {
                    return StartCauses.Toast;
                }
                else if (args is BackgroundActivatedEventArgs)
                {
                    return StartCauses.BackgroundTrigger;
                }
                else if (RawEventArgs is ILaunchActivatedEventArgs e && e != null)
                {
                    var defaultTileID = "App";
                    if (e.TileId.Equals(defaultTileID) && string.IsNullOrEmpty(e?.Arguments))
                    {
                        return StartCauses.Primary;
                    }
                    else if (e.TileId.Equals(defaultTileID) && !string.IsNullOrEmpty(e?.Arguments))
                    {
                        return StartCauses.JumpListItem;
                    }
                    else if (!string.IsNullOrEmpty(e?.TileId) && e?.TileId != defaultTileID)
                    {
                        return StartCauses.SecondaryTile;
                    }
                    else
                    {
                        return StartCauses.Other;
                    }
                }
                else
                {
                    return StartCauses.Other;
                }
            }
        }

        public bool FirstTime => Starts == 1;
        public static int Starts { get; private set; } = 0;
        public static bool PrelaunchOccurred { get; private set; }

        public object RawEventArgs { get; }
        public StartKinds StartKind { get; }
        public StartCauses StartCause { get; }
        public bool ThisIsPrelaunch { get; }
        public ApplicationExecutionState PreviousExecutionState
        {
            get
            {
                if (Starts > 1) return ApplicationExecutionState.Running;
                else if (OnActivatedEventArgs != null) return OnActivatedEventArgs.PreviousExecutionState;
                else return ApplicationExecutionState.NotRunning;
            }
        }
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
    }
}
