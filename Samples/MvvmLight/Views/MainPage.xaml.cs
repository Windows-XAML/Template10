using Windows.UI.Xaml.Controls;

namespace Sample.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public ViewModels.MainPageViewModel ViewModel
        {
            get { return DataContext as ViewModels.MainPageViewModel; }
        }
    }
}