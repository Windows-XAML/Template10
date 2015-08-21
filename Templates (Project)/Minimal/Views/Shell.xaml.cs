using Template10.Services.NavigationService;
using Windows.UI.Xaml.Controls;

namespace Minimal.Views
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SplitView
    public sealed partial class Shell : Page
    {
        public Shell(NavigationService navigationService)
        {
            this.InitializeComponent();
            MyHamburgerMenu.NavigationService = navigationService;
        }
    }
}
