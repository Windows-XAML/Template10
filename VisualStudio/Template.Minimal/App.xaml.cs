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

        public override Task OnInitializeAsync()
        {
            // this.RequestedTheme = Services.SettingsService.GetInstance().AppTheme;
            Settings.ShellBackButtonPreference = Services.SettingsService.GetInstance().ShellBackButtonPreference;
            Settings.CacheExpiry = Services.SettingsService.GetInstance().CacheExpiry;
            return base.OnInitializeAsync();
        }

        public override async Task<UIElement> CreateSpashAsync(SplashScreen e)
        {
            return new Views.Splash(e);
        }

        public override async Task OnStartAsync(IStartArgsEx e)
        {
            await NavigationService.NavigateAsync(typeof(Views.MainPage));
        }
    }
}
