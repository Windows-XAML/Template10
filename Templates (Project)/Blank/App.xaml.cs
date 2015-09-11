using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media;

namespace Blank
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-Bootstrapper
    /// https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-NavigationService

    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }
        
        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // This code sets up the title bar
            // To use the user's chosen accent color by default.
            // If you want to set your own defaults,
            // Just update this code block to match your chosen colors.
            
            
            ApplicationViewTitleBar titlebar = ApplicationView.GetForCurrentView().TitleBar;
            titlebar.BackgroundColor = (Color)Resources["SystemAccentColor"];
            titlebar.ForegroundColor = Colors.White;
            titlebar.InactiveBackgroundColor = (Resources["MediumAccentBrush"] as SolidColorBrush).Color;
            titlebar.ButtonInactiveBackgroundColor = (Resources["MediumAccentBrush"] as SolidColorBrush).Color;
            titlebar.InactiveForegroundColor = Colors.White;
            titlebar.ButtonInactiveForegroundColor = Colors.White;
            titlebar.ButtonHoverBackgroundColor = (Resources["MediumAccentBrush"] as SolidColorBrush).Color;
            titlebar.ButtonHoverForegroundColor = Colors.White;
            titlebar.ButtonPressedBackgroundColor = (Resources["DarkAccentBrush"] as SolidColorBrush).Color;
            titlebar.ButtonPressedForegroundColor = Colors.White;
            titlebar.ButtonBackgroundColor = (Color)Resources["SystemAccentColor"];
            titlebar.ButtonForegroundColor = Colors.White;
            
            NavigationService.Navigate(typeof(Views.MainPage));
            return Task.FromResult<object>(null);
        }
    }
}
