using Windows.UI.Xaml.Controls;

namespace Template10.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            this.DataContextChanged += (s, e) => ViewModel = DataContext as ViewModels.MainPageViewModel;
        }

        // strongly-typed view models enable x:bind
        public ViewModels.MainPageViewModel ViewModel { get; set; }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}