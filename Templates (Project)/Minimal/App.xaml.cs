using System;
using System.Threading.Tasks;
using Template10;
using Template10.Common;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Minimal
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
            ShowShellBackButton = true;
            CacheMaxDuration = TimeSpan.FromDays(2);
            SplashFactory = (e) => { return new Views.Splash(e); };
        }

        // runs even if restored from state
        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                Window.Current.Content = new Views.Shell(this.FrameFactory(true));
            }
            return base.OnInitializeAsync(args);
        }

        // runs unless restored from state
        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            Window.Current.Content = new Views.Shell(NavigationService);

            var launchArgs = args as ILaunchActivatedEventArgs;
            if ((launchArgs?.TileId ?? "App") == "App")
            {
                // launched via primary tile or toast
                NavigationService.Navigate(typeof(Views.MainPage));
            }
            else
            {
                // launched via a secondary tile
                NavigationService.Navigate(typeof(Views.DetailPage), launchArgs.Arguments);
            }
            return Task.FromResult<object>(null);
        }
    }
}
