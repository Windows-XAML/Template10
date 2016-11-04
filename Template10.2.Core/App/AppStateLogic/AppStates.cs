namespace Template10.App
{
    public enum AppStates
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