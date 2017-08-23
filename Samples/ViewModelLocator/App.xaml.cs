using System.Threading.Tasks;
using Template10.Services.Container.Unity.Demo.Strategies;
using Template10.BootStrap;
using Template10.Core;
using Template10.Services.Messenger;
using Template10.Strategies;
using Windows.UI.Xaml.Data;
using Template10.Services.Container.Unity.Demo.ViewModels;
using Template10.Services.Container.Unity.Demo.Views;

namespace Template10.Services.Container.Unity.Demo
{
    [Bindable]
    sealed partial class App : DefaultBootStrapper
    {

        public App()
            : base(new UnityContainerService())
        {
            InitializeComponent();
        }

        public override void RegisterMessagingService()
        {
            ContainerService.Register<IMessengerService, MvvmLightMessengerService>();
        }

        public override async Task OnInitializeAsync()
        {
            var settings = Services.SettingsService.GetInstance();
            Template10.Settings.DefaultTheme = settings.DefaultTheme;
            Template10.Settings.ShellBackButtonPreference = settings.ShellBackButtonPreference;
            Template10.Settings.CacheMaxDuration = settings.CacheMaxDuration;

            RegisterViewModel();
        }

        private void RegisterViewModel()
        {
            ContainerService.Register<ITemplate10ViewModel, MainPageViewModel>(nameof(MainPage));
            ContainerService.Register<ITemplate10ViewModel, DetailPageViewModel>(nameof(DetailPage));
            ContainerService.Register<ITemplate10ViewModel, SettingsPageViewModel>(nameof(SettingsPage));
        }

        public override async Task OnStartAsync(IStartArgsEx e)
        {
            await NavigationService.NavigateAsync(typeof(MainPage));
        }

        public override void RegisterDependencies()
        {
            ContainerService.Register<IViewModelResolutionStrategy, UnityViewModelResolutionStrategy>();
        }
    }
}
