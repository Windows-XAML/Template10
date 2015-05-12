using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace Blank3.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            this.DataContextChanged += (s, e) => ViewModel = DataContext as ViewModels.MainPageViewModel;
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Debug.WriteLine("Loaded");
            FindName("Special1");
            Special1.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }


        // strongly-typed view models enable x:bind
        public ViewModels.MainPageViewModel ViewModel { get; set; }
    }
}