using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Sample.Views
{
    public sealed partial class DetailPage : Page
    {
        public DetailPage()
        {
            InitializeComponent();
        }

        public ViewModels.DetailPageViewModel ViewModel
            => DataContext as ViewModels.DetailPageViewModel;
    }
}
