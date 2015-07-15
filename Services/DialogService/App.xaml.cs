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
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Launch)
            {
                var launchArgs = args as LaunchActivatedEventArgs;
                if (launchArgs.TileId != "My2Tile")
                {
                    // start the user experience
                    NavigationService.Navigate(typeof(Views.MainPage), launchArgs.Arguments);
                    return Task.FromResult<object>(null);
                }
            }

            // start the user experience
            NavigationService.Navigate(typeof(Views.MainPage));
            return Task.FromResult<object>(null);
        }
    }
}
