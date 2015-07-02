using Template10.ViewModels;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            DataContextChanged += (s, e) => ViewModel = DataContext as MainPageViewModel;
        }

        // strongly-typed view models enable x:bind
        public MainPageViewModel ViewModel { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}