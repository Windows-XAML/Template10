using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Template10;
using Template10.Core;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Sample.ViewModels;
using System;
using Windows.UI.Xaml.Controls;
using Template10.Strategies;
using Template10.Services.Container;
using Sample.Views;

namespace Sample
{
    [Bindable]
    sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public override UIElement CreateSpash(SplashScreen e)
        {
            return new Views.Splash(e);
        }

        public override UIElement CreateRootElement(IStartArgsEx e)
        {
            return base.CreateRootElement(e);
        }

        public override async Task OnInitializeAsync()
        {
            RegisterViewModels(Container);
            LoadSettings(Services.SettingsService.GetInstance());
        }

        public override async Task OnStartAsync(IStartArgsEx e)
        {
            await NavigationService.NavigateAsync(typeof(Views.MainPage));
        }

        private static void LoadSettings(Services.SettingsService settings)
        {
            Template10.Settings.DefaultTheme = settings.DefaultTheme;
            Template10.Settings.ShellBackButtonPreference = settings.ShellBackButtonPreference;
            Template10.Settings.CacheMaxDuration = settings.CacheMaxDuration;
        }

        private void RegisterViewModels(IContainerService service)
        {
            service.Register<IViewModelResolutionStrategy, CustomViewModelResolutionStrategy>();
            service.Register<ITemplate10ViewModel, MainPageViewModel>(typeof(MainPage).ToString());
            service.Register<ITemplate10ViewModel, DetailPageViewModel>(typeof(DetailPage).ToString());
            service.Register<ITemplate10ViewModel, SettingsPageViewModel>(typeof(SettingsPage).ToString());
        }
    }

    public class CustomViewModelResolutionStrategy : IViewModelResolutionStrategy
    {
        IContainerService container = ContainerService.Default;

        public async Task<object> ResolveViewModelAsync(Type type)
            => container.Resolve<ITemplate10ViewModel>(type.ToString());

        public async Task<object> ResolveViewModelAsync(Page page)
            => await ResolveViewModelAsync(page.GetType());
    }
}
