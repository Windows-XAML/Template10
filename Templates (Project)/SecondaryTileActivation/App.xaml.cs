using SecondaryTileActivation.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SecondaryTileActivation
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : BootStrapper
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            switch(startKind)
            {
                case StartKind.Launch:
                    var launchArgs = args as ILaunchActivatedEventArgs;
                    if (launchArgs != null && launchArgs.TileId != null && launchArgs.TileId != "App")
                    {
                        // launched via a secondary tile
                        NavigationService.Navigate(typeof(SecondPage), launchArgs.Arguments);
                    }
                    else
                    {
                        NavigationService.Navigate(typeof(MainPage));
                    }
                    break;
            }

            return Task.FromResult<object>(null); 
        }
    }
}
