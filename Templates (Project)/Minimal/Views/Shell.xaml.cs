using Template10.Services.NavigationService;
using Windows.UI.Xaml.Controls;

namespace Minimal.Views
{
    public sealed partial class Shell : Page
    {
        public Shell(NavigationService navigationService)
        {
            this.InitializeComponent();
            MyHamburgerMenu.NavigationService = navigationService;
        }
    }
}
