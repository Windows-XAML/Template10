namespace Template10.App
{
    public enum BootstrapperStates
    {
        None,
        Running,

        BeforeInitializeAsync,
        AfterInitializeAsync,

        BeforeInternalPrelaunch,
        AfterInternalPrelaunch,

        BeforeStartAsync,
        AfterStartAsync,

        BeforeInternalLaunch,
        AfterInternalLaunch,

        BeforeInternalActivate,
        AfterInternalActivate,
    }
}