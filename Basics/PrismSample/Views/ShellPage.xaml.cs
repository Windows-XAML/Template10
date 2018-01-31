using Template10.Navigation;
using Windows.UI.Xaml.Controls;

namespace PrismSample.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellPage(out INavigationServiceUwp navigationService)
        {
            InitializeComponent();
            navigationService = ShellView.Start();
        }
    }
}
