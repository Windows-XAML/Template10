using T102.ViewModelLocator.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace T102.ViewModelLocator.Views
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