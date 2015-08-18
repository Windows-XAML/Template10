using Template10.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Template10.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        // strongly-typed view models enable x:bind
        public SettingsPageViewModel ViewModel { get { return this.DataContext as SettingsPageViewModel; } }
    }
}
