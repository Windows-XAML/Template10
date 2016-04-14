using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Samples.SearchSample.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void ShowBusyClick(object sender, RoutedEventArgs e)
        {
            Shell.ShowBusy(true, "Please wait...");
            WindowWrapper.Current().Dispatcher.Dispatch(() => Shell.ShowBusy(false, string.Empty), 5000);
        }
    }
}
