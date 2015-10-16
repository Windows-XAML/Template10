using Sample.ViewModels;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;

namespace Sample.Views
{
    public sealed partial class BabPage : Page
    {
        public BabPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        // strongly-typed view models enable x:bind
        public BabPageViewModel ViewModel => DataContext as BabPageViewModel;
    }
}
