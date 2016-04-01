using Template10.Samples.MvvmLightSample.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Template10.Samples.MvvmLightSample.Views
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