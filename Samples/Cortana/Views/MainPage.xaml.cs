using BottomAppBar.ViewModels;
using Windows.UI.Xaml.Controls;

namespace BottomAppBar.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public MainPageViewModel ViewModel => this.DataContext as MainPageViewModel;
    }
}