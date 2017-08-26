using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MultiplePageHeaders.Views
{
    public sealed partial class DetailPage : Page
    {
        public DetailPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        private void Frame_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}

