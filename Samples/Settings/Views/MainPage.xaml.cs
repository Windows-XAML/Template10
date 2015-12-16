using Template10.Common;
using Template10.Utils;
using Windows.UI.Xaml.Controls;

namespace Sample.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            if (DeviceUtils.Current(WindowWrapper.Current()).DeviceDisposition() == DeviceUtils.DeviceDispositions.Desktop)
            {
                ViewModel.DesktopOnlyVisibility = true;
            }
        }

        public ViewModels.MainPageViewModel ViewModel => DataContext as ViewModels.MainPageViewModel;
    }
}
