using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sample.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        ViewModels.MainPageViewModel ViewModel
        {
            get { return this.DataContext as ViewModels.MainPageViewModel; }
        }
    }
}