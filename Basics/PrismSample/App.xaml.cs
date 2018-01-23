using System.Threading.Tasks;
using Template10.Application;
using Template10.Navigation;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace PrismSample
{
    public enum Pages { MainPage }

    sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public static NavigationService NavigationService;

        public override void Initialize(StartArgs args)
        {
            Window.Current.Content = new Views.ShellPage(out NavigationService);
            NavigationService.PageRegistry.Add(Pages.MainPage.ToString(), (typeof(Views.MainPage), null));
        }

        public override async Task StartAsync(StartArgs args, StartKinds activate)
        {
            NavigationService.SessionState.Add("Identity", new object());
            var path = new NavigationPath(Pages.MainPage.ToString(), true)
                .AddParameter(("Value", "123"), ("Key", "234"), ("Id", "345"))
                .AddParameter((nameof(NavigationService.SessionState), "Identity"));
            await NavigationService.NavigateAsync(path.ToString());
        }
    }
}
