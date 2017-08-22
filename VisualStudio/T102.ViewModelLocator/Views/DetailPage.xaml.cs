using T102.ViewModelLocator.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace T102.ViewModelLocator.Views
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
