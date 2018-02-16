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
            if (args.StartKind == StartKinds.Launch && args.Arguments is ILaunchActivatedEventArgs e)
            {
                Window.Current.Content = new SplashPage(e.SplashScreen);
            }

            var path = PathBuilder
                .Create(true, nameof(MainPage), ("Record", "123"))
                .Append(nameof(MainPage), ("Record", "567"))
                .ToString();

            var shell = Central.Container.Resolve<ShellPage>();
            await shell.ShellView.NavigationService.NavigateAsync(path);
            Window.Current.Content = shell;
        }
    }
}
