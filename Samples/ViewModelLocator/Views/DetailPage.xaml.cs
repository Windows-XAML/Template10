using Template10.Services.Container.Unity.Demo.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.Container.Unity.Demo.Views
{
    public sealed partial class DetailPage : Page
    {
        public DetailPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        DetailPageViewModel ViewModel => DataContext as DetailPageViewModel;
    }
}
