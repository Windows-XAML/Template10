using Windows.UI.Xaml.Controls;

namespace Template10.Samples.SettingsSample.Views
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
