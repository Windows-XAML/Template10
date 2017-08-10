using Windows.UI.Core;

namespace Template10.Services.Gesture
{
    public static partial class Settings
    {
        internal static bool ShellBackButtonVisible
        {
            get => SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility == AppViewBackButtonVisibility.Visible ? true : false;
            set => SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = value ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        public static ShellBackButtonPreferences ShellBackButtonPreference { get; set; } = ShellBackButtonPreferences.AutoShowInShell;
    }

    public enum ShellBackButtonPreferences { AlwaysShowInShell, AutoShowInShell, NeverShowInShell }
}
