using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI;
using Windows.UI.ViewManagement;

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
            NavigationService.Navigate(typeof(Views.MainPage));
            return Task.FromResult<object>(null);
        }
    }
}
