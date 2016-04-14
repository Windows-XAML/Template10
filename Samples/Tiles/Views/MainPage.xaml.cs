using Template10.Samples.TilesSample.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Template10.Samples.TilesSample.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
        }

        // strongly-typed view models enable x:bind
        public MainPageViewModel ViewModel => this.DataContext as MainPageViewModel; 
    }
}