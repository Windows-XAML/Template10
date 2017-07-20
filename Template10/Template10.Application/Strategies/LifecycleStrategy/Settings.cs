namespace Template10.Strategies
{
    public static partial class Settings
    {
        public static bool RunSuspendStrategy { get; set; } = true;
        public static bool RunRestoreStrategy { get; set; } = true;

        /// <summary>
        /// Every time the app starts, try to resume
        /// </summary>
        public static bool AppAlwaysResumes { get; set; } = true;
    }
}
