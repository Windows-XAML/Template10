using Template10.Services.Container.Unity.Demo.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.Container.Unity.Demo.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        MainPageViewModel ViewModel => DataContext as MainPageViewModel;
    }
}