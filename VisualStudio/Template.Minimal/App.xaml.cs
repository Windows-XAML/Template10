using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using System.Diagnostics;
using Template10;
using Template10.StartArgs;
using static Template10.StartArgs.Template10StartArgs;

namespace Sample
{
    [Bindable]
    sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();

            //SettingsService _settings = SettingsService.Instance;
            //RequestedTheme = _settings.AppTheme;
            //// Settings.ShowShellBackButton = _settings.UseShellBackButton;
            //Settings.SplashFactory = s => new Views.Splash(s);
            //Settings.CacheMaxDuration = _settings.CacheMaxDuration;
            //Settings.AutoExtendExecutionSession = false;
        }

        public override async Task OnStartAsync(ITemplate10StartArgs e)
        {
            Windows.UI.Xaml.Window.Current.VisibilityChanged += (s, args) =>
            {
                Debug.WriteLine($"Window Visible: {args.Visible}");
            };

            switch (e.StartKind)
            {
                case StartKinds.Prelaunch:
                case StartKinds.Launch:
                case StartKinds.Activate:
                    switch (e.StartCause)
                    {
                        case StartCauses.Primary:
                        case StartCauses.Toast:
                        case StartCauses.SecondaryTile:
                        case StartCauses.JumpListItem:
                        case StartCauses.BackgroundTrigger:
                        case StartCauses.CommandLine:
                        case StartCauses.Undetermined:
                        default:
                            await NavigationService.NavigateAsync(typeof(Views.MainPage));
                            break;
                    }
                    break;
                case StartKinds.Background:
                    break;
            }
        }
    }
}
