using Minimal.Services.SettingsServices;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Minimal
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-Bootstrapper
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();

            // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-Cache
            CacheMaxDuration = TimeSpan.FromDays(2);

            // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-BackButton
            ShowShellBackButton = SettingsService.Instance.UseShellBackButton;

            // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SplashScreen
            SplashFactory = (e) => new Views.Splash(e);
        }

        // runs even if restored from state
        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SplitView
            var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
            Window.Current.Content = new Views.Shell(nav);

            return Task.FromResult<object>(null);
        }

        // runs only when not restored from state
        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // simulated long-running load on startup
            await Task.Delay(5000);

            // start user experience
            switch (DecipherStartCause(args))
            {
                // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-Toast
                case AdditionalKinds.Toast:

                // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SecondaryTile
                case AdditionalKinds.SecondaryTile:
                    var e = (args as ILaunchActivatedEventArgs);
                    NavigationService.Navigate(typeof(Views.DetailPage), e.Arguments);
                    break;

                // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-NavigationService
                default:
                    NavigationService.Navigate(typeof(Views.MainPage));
                    break;
            }
        }
    }
}
