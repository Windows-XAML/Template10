using System;
using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Sample
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App() => InitializeComponent();

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
            Window.Current.Activate();
        }

        //public async override Task OnStartAsync(StartupInfo e)
        //{
        //    await NavigationService.NavigateAsync(typeof(Views.MainPage));
        //}
    }
}
