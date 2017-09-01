using Windows.UI.Xaml.Controls;

namespace Demo
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var service = new Template10.Services.Profile.UserService();
            var user = await service.GetUserExAsync();
            DataContext = user;
        }
    }
}
