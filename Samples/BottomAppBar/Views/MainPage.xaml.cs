using Template10.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sample.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            var ham = Shell.HamburgerMenu;
            ham.PaneOpened += (s, e) => UpdateMargin(ham);
            ham.PaneClosed += (s, e) => UpdateMargin(ham);
            UpdateMargin(ham);
        }

        private void UpdateMargin(HamburgerMenu ham)
        {
            var value = (ham.IsOpen) ? ham.PaneWidth : 48d;
            BottomAppBar.Margin = new Thickness(value, 0, 0, 0);
        }

private void SampleClick(object sender, RoutedEventArgs e)
{
    Shell.SetBusy(true, "Please wait...");
    BottomAppBar.IsEnabled = false;
    Template10.Common.WindowWrapper.Current().Dispatcher.Dispatch(() =>
    {
        Shell.SetBusy(false);
        BottomAppBar.IsEnabled = true;
    }, 3000);
}
    }
}
