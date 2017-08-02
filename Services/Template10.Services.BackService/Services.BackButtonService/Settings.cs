using Windows.UI.Core;

namespace Template10.Services.BackButtonService
{
    public static class Settings
    {
        public static bool ShellBackButtonVisible
        {
            get => SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility == AppViewBackButtonVisibility.Visible ? true : false;
            set => SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = value ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        public static bool ForceShowShellBackButton { get; set; } = false;
    }
}
