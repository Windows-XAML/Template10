using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Data;

namespace Sample
{
    [Bindable]
    sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();
            RequestedTheme = Windows.UI.Xaml.ApplicationTheme.Light;
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            await NavigationService.NavigateAsync(typeof(Views.MainPage));
        }
    }
}
