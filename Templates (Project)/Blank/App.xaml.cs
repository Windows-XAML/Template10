using System;
using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel.Activation;

namespace Template10
{
    sealed partial class App : Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
            this.SplashFactory = (e) => null;
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            if ((args.Kind == ActivationKind.Launch) && ((args as LaunchActivatedEventArgs)?.PrelaunchActivated ?? false))
            {
                // update pre-launch live tile
            }

            // start the user experience
            NavigationService.Navigate(typeof(Views.MainPage));
            return Task.FromResult<object>(null);
        }
    }
}
