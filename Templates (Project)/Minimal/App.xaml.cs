using Minimal.Services.SettingsServices;
using System;
using System.Threading.Tasks;
using Template10;
using Template10.Common;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Minimal
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
            CacheMaxDuration = TimeSpan.FromDays(2);
            ShowShellBackButton = SettingsService.Instance.UseShellBackButton;
            SplashFactory = (e) => { return new Views.Splash(e); };
        }

        // runs even if restored from state
        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            // use this when method not marked async
            return Task.FromResult<object>(null);
        }

        // runs only when not restored from state
        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // use splitview shell instead of default
            Window.Current.Content = new Views.Shell(NavigationService);

            // start user experience
            switch (DecipherStartCause(args))
            {
                case AdditionalKinds.Toast:
                case AdditionalKinds.SecondaryTile:
                    var e = (args as ILaunchActivatedEventArgs);
                    NavigationService.Navigate(typeof(Views.DetailPage), e.Arguments);
                    break;
                default:
                    NavigationService.Navigate(typeof(Views.MainPage));
                    break;
            }

            // use this when method not marked async
            return Task.FromResult<object>(null);
        }
    }
}
