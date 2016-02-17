using Messaging.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Messaging.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        public MainPageViewModel ViewModel => (DataContext as MainPageViewModel);
    }
}