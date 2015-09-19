using Template10.Services.NavigationService;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sample.Views
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SplitView
    public sealed partial class Shell : Page
    {
        private static Shell Instance { get; set; }
        private static Template10.Common.WindowWrapper Window { get; set; }

        public Shell(NavigationService navigationService)
        {
            Instance = this;
            this.InitializeComponent();
            Window = Template10.Common.WindowWrapper.Current();
            MyHamburgerMenu.NavigationService = navigationService;
            CustomizeTitleBarColors();
        }

        public static void SetBusyIndicator(bool busy, string text = null)
        {
            Window.Dispatcher.Dispatch(() =>
            {
                Instance.BusyIndicator.Visibility = (busy)
               ? Visibility.Visible : Visibility.Collapsed;
                Instance.BusyRing.IsActive = busy;
                Instance.BusyText.Text = text ?? string.Empty;
            });
        }

        private static void CustomizeTitleBarColors()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = SetColorFromResources(titleBar.BackgroundColor, "TitleBarBackground");
            titleBar.ForegroundColor = SetColorFromResources(titleBar.ForegroundColor, "TitleBarForeground");
            titleBar.ButtonBackgroundColor = SetColorFromResources(titleBar.ButtonBackgroundColor, "TitleBarButtonBackground");
            titleBar.ButtonForegroundColor = SetColorFromResources(titleBar.ButtonForegroundColor, "TitleBarButtonForeground");
            titleBar.ButtonHoverBackgroundColor = SetColorFromResources(titleBar.ButtonHoverBackgroundColor, "TitleBarButtonHoverBackground");
            titleBar.ButtonHoverForegroundColor = SetColorFromResources(titleBar.ButtonHoverForegroundColor, "TitleBarButtonHoverForeground");
            titleBar.ButtonPressedBackgroundColor = SetColorFromResources(titleBar.ButtonPressedBackgroundColor, "TitleBarButtonPressedBackground");
            titleBar.ButtonPressedForegroundColor = SetColorFromResources(titleBar.ButtonPressedForegroundColor, "TitleBarButtonPressedForeground");
            titleBar.ButtonInactiveBackgroundColor = SetColorFromResources(titleBar.ButtonInactiveBackgroundColor, "TitleBarButtonInactiveBackground");
            titleBar.ButtonInactiveForegroundColor = SetColorFromResources(titleBar.ButtonInactiveForegroundColor, "TitleBarButtonInactiveForeground");
            titleBar.InactiveBackgroundColor = SetColorFromResources(titleBar.InactiveBackgroundColor, "TitleBarInactiveBackground");
            titleBar.InactiveForegroundColor = SetColorFromResources(titleBar.InactiveForegroundColor, "TitleBarInactiveForeground");
        }

        private static Color? SetColorFromResources(Color? actualColor, string resourceName)
        {
            try
            {
                if (Application.Current.Resources[resourceName] is Color)
                {
                    return (Color)Application.Current.Resources[resourceName];
                }
            }
            catch {}

            return actualColor;
        }

    }
}
