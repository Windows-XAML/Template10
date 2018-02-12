using System.Threading.Tasks;
using PrismSample.Views;
using Prism.Windows;
using Prism.Windows.Navigation;
using Windows.UI.Xaml;
using Prism.Ioc;
using PrismSample.ViewModels;
using Windows.ApplicationModel.Activation;
using PrismSample.Services;

namespace PrismSample
{
    sealed partial class App : PrismApplication
    {
        public App()
        {
            InitializeComponent();
        }

        public override void RegisterTypes(IContainerRegistry container)
        {
            container.RegisterSingleton<ShellPage, ShellPage>();
            container.RegisterSingleton<IDataService, DataService>();
            container.RegisterForNavigation<MainPage, MainPageViewModel>();
        }

        public override async Task OnStartAsync(StartArgs args, StartKinds activate)
        {
            switch (activate)
            {
                case StartKinds.Launch when (args.Arguments is LaunchActivatedEventArgs e):
                    Window.Current.Content = new SplashPage(e.SplashScreen);
                    break;
                case StartKinds.Prelaunch:
                case StartKinds.Activate:
                case StartKinds.Background:
                    break;
            }

            var path = PathBuilder
                .Create(true, nameof(MainPage), ("Record", "123"))
                .Append(nameof(MainPage), ("Record", "234"))
                .Append(nameof(MainPage), ("Record", "345"))
                .Append(nameof(MainPage), ("Record", "456"))
                .Append(nameof(MainPage), ("Record", "567"))
                .ToString();

            var shell = Resolve<ShellPage>();
            await shell.ShellView.NavigationService.NavigateAsync(path);
            Window.Current.Content = shell;
        }
    }
}
