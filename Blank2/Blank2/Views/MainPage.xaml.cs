using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Blank2.Views
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
            FindName("Rec6");
            Rec6.Visibility = Windows.UI.Xaml.Visibility.Visible;  
            FindName("Rec7");  
            FindName("Rec8");  
        }


        // strongly-typed view models enable x:bind
        public ViewModels.MainPageViewModel ViewModel { get; set; }
    }
}