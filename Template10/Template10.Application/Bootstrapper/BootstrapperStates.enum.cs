namespace Template10.Common
{
    public enum BootstrapperStates
    {
        None,

        Launching,
        Launched,

        Starting,
        Started,

        Activating,
        Activated,

        Prelaunching,
        Prelaunched,

        Initializing,
        Initialized,

        Restoring,
        Restored,

        Resuming,
        Resumed,

        CreatingRootElement,
        CreatedRootElement,

        Suspending,
        Suspended,
        HidingSplash,
        HiddenSplash,
        ShowingSplash,
        ShowedSplash,
    }
}
