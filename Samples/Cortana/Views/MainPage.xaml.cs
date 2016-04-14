using Template10.Samples.CortanaSample.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Template10.Samples.CortanaSample.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public MainPageViewModel ViewModel => this.DataContext as MainPageViewModel;

        private void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}