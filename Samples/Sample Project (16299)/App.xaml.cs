using System.Threading.Tasks;
using Sample.Views;
using Prism;
using Prism.Navigation;
using Windows.UI.Xaml;
using Prism.Ioc;
using Sample.ViewModels;
using Windows.ApplicationModel.Activation;
using Sample.Services;
using Prism.Logging;
using Prism.Events;
using Template10.Services.Dialog;
using Template10.Services.Compression;
using Template10.Services.File;
using Template10.Services.Marketplace;
using Template10.Services.Nag;
using Template10.Services.Network;
using Template10.Services.Resources;
using Template10.Services.Secrets;
using Template10.Services.Serialization;
using Template10.Services.Settings;
using Template10.Services.Web;
using Unity;
using Prism.Unity;

namespace Sample
{
    sealed partial class App : PrismApplication
    {
        public App()
        {
            InitializeComponent();
        }

        public override void RegisterTypes(IContainerRegistry container)
        {
            // standard template 10 services

            container.RegisterSingleton<ICompressionService, CompressionService>();
            container.RegisterSingleton<IDialogService, DialogService>();
            container.RegisterSingleton<IFileService, FileService>();
            container.RegisterSingleton<IMarketplaceService, MarketplaceService>();
            container.RegisterSingleton<INagService, NagService>();
            container.RegisterSingleton<INetworkService, NetworkService>();
            container.RegisterSingleton<IResourceService, ResourceService>();
            container.RegisterSingleton<ISecretService, SecretService>();
            container.RegisterSingleton<ISettingsHelper, SettingsHelper>();
            container.RegisterSingleton<IWebApiService, WebApiService>();

            // custom services

            container.RegisterSingleton<ISerializationService, NewtonsoftSerializationService>();

            // pages and view-models

            container.RegisterSingleton<ShellPage, ShellPage>();
            container.RegisterSingleton<IDataService, DataService>();
            container.RegisterForNavigation<MainPage, MainPageViewModel>(nameof(MainPage));
            container.RegisterForNavigation<SearchPage, SearchPageViewModel>(nameof(SearchPage));
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
                .Create(nameof(MainPage), ("Record", "0567"))
                .Append(nameof(MainPage), ("Record", "1567"))
                .Append(nameof(MainPage), ("Record", "2567"))
                .Append(nameof(MainPage), ("Record", "3567"))
                .Append(nameof(MainPage), ("Record", "4567"))
                .Append(nameof(MainPage), ("Record", "5567"))
                .Append(nameof(MainPage), ("Record", "6567"))
                .ToString();

            // initialize services

            var data = Container.Resolve<IDataService>();
            await data.InitializeAsync();

            // setup shell

            var shell = Container.Resolve<ShellPage>();
            Window.Current.Content = shell;

            // first page 

            var navigationService = shell.ShellView.NavigationService;
            await navigationService.NavigateAsync(path);
        }
    }
}
