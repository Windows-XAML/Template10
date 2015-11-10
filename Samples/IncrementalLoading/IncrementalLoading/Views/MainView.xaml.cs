using BottomAppBar.ViewModels;
using Windows.UI.Xaml.Controls;

namespace BottomAppBar.Views
{
    public sealed partial class MainView : Page
    {
        public MainView()
        {
            this.InitializeComponent();
        }

        public MainViewModel ViewModel
        {
            get
            {
                return this.DataContext as MainViewModel;
            }
        }
    }
}