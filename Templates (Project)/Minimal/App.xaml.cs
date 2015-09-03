using Minimal.Services.SettingsServices;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Minimal
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-Bootstrapper
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-Cache
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-BackButton
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SplashScreen
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SplitView
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-NavigationService

    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
            CacheMaxDuration = TimeSpan.FromDays(2);
            ShowShellBackButton = SettingsService.Instance.UseShellBackButton;
            SplashFactory = (e) => new Views.Splash(e);
        }

        // runs even if restored from state
        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
            Window.Current.Content = new Views.Shell(nav);
            return Task.FromResult<object>(null);
        }

        // runs only when not restored from state
        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            await Task.Delay(50);
            NavigationService.Navigate(typeof(Views.MainPage));
        }
    }
}
