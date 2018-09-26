using System.Threading.Tasks;
using Template10.Utilities;
using Sample.Views;
using Prism;
using Prism.Navigation;
using Windows.UI.Xaml;
using Prism.Ioc;
using Sample.ViewModels;
using Windows.ApplicationModel.Activation;
using Sample.Services;
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
using Windows.Storage;
using System;

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
            container.RegisterTemplate10Services();
            container.RegisterSingleton<ShellPage, ShellPage>();
            container.RegisterSingleton<IDataService, DataService>();
            container.RegisterForNavigation<MainPage, MainPageViewModel>(nameof(MainPage));
            container.RegisterForNavigation<SearchPage, SearchPageViewModel>(nameof(SearchPage));
        }

        public override void OnInitialized()
        {
            // empty
        }

        public override async Task OnStartAsync(StartArgs args)
        {
            if (args.StartKind == StartKinds.Resume)
            {
                if (args.Arguments is IResumeArgs resume)
                {
                    if ((DateTime.Now - resume.SuspensionDate) < TimeSpan.FromHours(1))
                    {
                        // continue with resume
                    }
                    else
                    {
                        // start over
                    }
                }
            }

            // built initial path

            var navigationPath = string.Empty;

            // handle startup

            switch (args.StartKind)
            {
                case StartKinds.Launch when (args.Arguments is ILaunchActivatedEventArgs e):
                    navigationPath = PathBuilder.Create(nameof(MainPage), ("Record", "0567")).ToString();
                    Window.Current.Content = new SplashPage(e.SplashScreen);
                    break;
                case StartKinds.Resume when (args.Arguments is ResumeArgs resume 
                        && ApplicationData.Current.LocalSettings.Values.TryGetValue(nameof(Suspending), out navigationPath)):
                    ApplicationData.Current.LocalSettings.Values.Remove(nameof(Suspending));
                    if ((DateTime.Now - resume.SuspensionDate) < TimeSpan.FromHours(1))
                    {
                        return;
                    }
                    break;
                case StartKinds.Prelaunch:
                case StartKinds.Activate:
                case StartKinds.Background:
                    return;
            }

            // initialize services

            var data = Container.Resolve<IDataService>();
            await data.InitializeAsync();

            // setup shell

            var shell = Container.Resolve<ShellPage>();
            Window.Current.Content = shell;

            // first page 

            var navigationService = shell.ShellView.NavigationService;
            await navigationService.NavigateAsync(navigationPath);
        }

        public override void OnSuspending()
        {
            if (Window.Current.Content is ShellPage page)
            {
                var path = page.ShellView.NavigationService.GetNavigationPath(true);
                ApplicationData.Current.LocalSettings.Values.ForceAdd(nameof(Suspending), path);
            }
        }
    }
}
