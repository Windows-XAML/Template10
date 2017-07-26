using System;
using System.Threading.Tasks;
using Template10.Common;
using Template10.StartArgs;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Sample
{
    sealed partial class App : Template10.BootStrapper
    {
        public App() => InitializeComponent();

        public override async Task OnStartAsync(ITemplate10StartArgs e)
        {
            await NavigationService.NavigateAsync(typeof(Views.MainPage));
        }

        //public async override Task OnStartAsync(StartupInfo e)
        //{
        //    await NavigationService.NavigateAsync(typeof(Views.MainPage));
        //}
    }
}
