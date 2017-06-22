using System;
using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel.Activation;

namespace Sample
{
    sealed partial class App : BootStrapper
    {
        public App() => InitializeComponent();

        public async override Task OnStartAsync(StartupInfo e)
        {
            await NavigationService.NavigateAsync(typeof(Views.MainPage));
        }
    }
}
