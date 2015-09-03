using Template10.Services.NavigationService;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Minimal.Views
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
    }
}
