using System.Threading.Tasks;
using PrismSample.Views;
using Prism.Windows;
using Prism.Windows.Navigation;
using Windows.UI.Xaml;
using Prism.Ioc;
using PrismSample.ViewModels;
using Windows.ApplicationModel.Activation;
using PrismSample.Services;
using System.Diagnostics;
using Template10;

namespace PrismSample
{
    sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public override void RegisterTypes(IContainerRegistry container)
        {
            container.RegisterSingleton<ShellPage, ShellPage>();
            container.RegisterSingleton<IDataService, DataService>();
            container.RegisterForNavigation<MainPage, MainPageViewModel>(nameof(MainPage));
        }

        public override async Task OnStartAsync(StartArgs args)
        {
            // show splash

            if (args.StartKind == StartKinds.Launch && args.Arguments is ILaunchActivatedEventArgs e)
            {
                Window.Current.Content = new SplashPage(e.SplashScreen);
            }

            // built initial path

            var path = PathBuilder
                .Create(true, nameof(MainPage), ("Filter", "Plumber"))
                .Append(nameof(MainPage), ("Record", "567"), ("ScrollPosition", "234"))
                .ToString();

            // do initial navigation (hide splash)

            var shell = Central.Container.Resolve<ShellPage>();
            await shell.ShellView.NavigationService.NavigateAsync(path);
            Window.Current.Content = shell;
        }
    }

    public class DetailPage : PageRegistry { }
}
