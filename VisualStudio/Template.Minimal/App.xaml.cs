using System.Threading.Tasks;
using Sample.Services.SettingsServices;
using Template10.Common;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using Windows.UI;
using Template10.Utils;

namespace Sample
{
    [Bindable]
    sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();
            SettingsService _settings = SettingsService.Instance;
            RequestedTheme = _settings.AppTheme;
            // Settings.ShowShellBackButton = _settings.UseShellBackButton;
            Settings.SplashFactory = s => new Views.Splash(s);
            Settings.CacheMaxDuration = _settings.CacheMaxDuration;
            Settings.AutoExtendExecutionSession = false;
        }

        public async override Task OnStartAsync(StartupInfo e)
        {
            switch (e.StartKind)
            {
                case StartKinds.Prelaunch:
                case StartKinds.Launch:
                case StartKinds.Activate:
                    await NavigationService.NavigateAsync(typeof(Views.MainPage));
                    break;
                case StartKinds.Background:
                    break;
            }
        }
    }
}
