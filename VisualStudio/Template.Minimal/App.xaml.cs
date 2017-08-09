using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Template10;
using Template10.Core;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Sample
{
    [Bindable]
    sealed partial class App : BootStrapper
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
            Settings.CacheExpiry = settings.CacheExpiry;
        }

        public override UIElement CreateRootElement(IStartArgsEx e)
        {
            return base.CreateRootElement(e);
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
