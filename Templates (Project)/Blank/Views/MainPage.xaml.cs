using Template10.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Template10.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            DataContextChanged += (s, e) => ViewModel = DataContext as MainPageViewModel;
        }

        // strongly-typed view models enable x:bind
        public MainPageViewModel ViewModel { get; set; }
    }
}