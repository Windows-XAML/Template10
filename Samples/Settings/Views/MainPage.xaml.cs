using Windows.UI.Xaml.Controls;

namespace Messaging.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public ViewModels.MainPageViewModel ViewModel => DataContext as ViewModels.MainPageViewModel;
    }
}
