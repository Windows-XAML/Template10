using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Template10;
using Template10.Core;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Template10.Services.Container;

namespace Sample
{
    [Bindable]
    sealed partial class App : MvvmLightBootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public override async Task OnInitializeAsync()
        {
            var settings = Services.SettingsService.GetInstance();
            Settings.DefaultTheme = settings.DefaultTheme;
            Settings.ShellBackButtonPreference = settings.ShellBackButtonPreference;
            Settings.CacheMaxDuration = settings.CacheMaxDuration;
        }

        public override UIElement CreateSpash(SplashScreen e)
        {
            return new Views.Splash(e);
        }

        public override async Task OnStartAsync(IStartArgsEx e)
        {
            await NavigationService.NavigateAsync(typeof(Views.MainPage));
        }
    }
}
