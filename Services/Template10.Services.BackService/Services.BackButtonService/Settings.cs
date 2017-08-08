using Windows.UI.Core;

namespace Template10.Services.BackButtonService
{
    public static class Settings
    {
        internal static bool ShellBackButtonVisible
        {
            get => SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility == AppViewBackButtonVisibility.Visible ? true : false;
            set => SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = value ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        static ShellBackButtonPreferences _ShellBackButtonPreference = ShellBackButtonPreferences.AutoShowInShell;
        public static ShellBackButtonPreferences ShellBackButtonPreference
        {
            get => _ShellBackButtonPreference;
            set
            {
                _ShellBackButtonPreference = value;
                BackButtonService.GetDefault().UpdateBackButton(false);
            }
        }
    }

    public enum ShellBackButtonPreferences { AlwaysShowInShell, AutoShowInShell, NeverShowInShell }
}
