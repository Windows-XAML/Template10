using System;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using Sample.Services.SettingsServices;
using Windows.ApplicationModel.Activation;
using Windows.UI.ViewManagement;
using Windows.UI;

namespace Sample
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-Bootstrapper
    /// https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-Cache
    /// https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-BackButton
    /// https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SplashScreen
    /// https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SplitView
    /// https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-NavigationService

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

            // This code sets up the title bar
            // To use the user's chosen accent color by default.
            // If you want to set your own defaults,
            // Just update this code to match your chosen colors.
            // In progress:
            // Needs good defaults for inactive, button hover, button click colors.
            // Preferably, these should work well with as many colors as possible.
            // Maybe find a way to darken the accent color automatically?
            ApplicationViewTitleBar titlebar = ApplicationView.GetForCurrentView().TitleBar;
            titlebar.BackgroundColor = (Color)Resources["SystemAccentColor"];
            titlebar.ForegroundColor = Colors.White;
            titlebar.ButtonBackgroundColor = (Color)Resources["SystemAccentColor"];
            titlebar.ButtonForegroundColor = Colors.White;
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
