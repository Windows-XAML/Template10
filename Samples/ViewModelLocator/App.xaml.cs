using System.Threading.Tasks;
using Template10.Services.Container.Unity.Demo.Strategies;
using Template10.BootStrap;
using Template10.Core;
using Template10.Mvvm;
using Template10.Services.Gesture;
using Template10.Services.Logging;
using Template10.Services.Messenger;
using Template10.Services.Resources;
using Template10.Services.Serialization;
using Template10.Strategies;

namespace Template10.Services.Container.Unity.Demo
{

    sealed partial class App : BootStrapperBase
    {

        public App()
        {
            InitializeComponent();
        }

        public override async Task OnInitializeAsync()
        {
            var settings = Services.SettingsService.GetInstance();
            Template10.Settings.DefaultTheme = settings.DefaultTheme;
            Template10.Settings.ShellBackButtonPreference = settings.ShellBackButtonPreference;
            Template10.Settings.CacheMaxDuration = settings.CacheMaxDuration;
        }

        public override async Task OnStartAsync(IStartArgsEx e)
        {
            await NavigationService.NavigateAsync(typeof(Views.MainPage));
        }

        public override void RegisterDependencies()
        {
            var c = Template10.Services.Container.ContainerService.Default = new UnityContainerService();

            // services
            c.Register<IMessengerService, MvvmLightMessengerService>();
            c.Register<ISessionState, SessionState>();
            c.Register<ILoggingService, LoggingService>();
            c.Register<ISerializationService, JsonSerializationService>();
            c.Register<IBackButtonService, BackButtonService>();
            c.Register<KeyboardService.IKeyboardService, KeyboardService.KeyboardService>();
            c.Register<Gesture.IKeyboardService, Gesture.KeyboardService>();
            c.Register<IGestureService, GestureService>();
            c.Register<IResourceService, ResourceService>();

            // strategies
            c.Register<IBootStrapperStrategy, DefaultBootStrapperStrategy>();
            c.RegisterInstance<IBootStrapperShared>(this);
            c.Register<ILifecycleStrategy, DefaultLifecycleStrategy>();
            c.Register<IStateStrategy, DefaultStateStrategy>();
            c.Register<ITitleBarStrategy, DefaultTitleBarStrategy>();
            c.Register<IExtendedSessionStrategy, DefaultExtendedSessionStrategy>();
            c.Register<IViewModelActionStrategy, DefaultViewModelActionStrategy>();
            c.Register<IViewModelResolutionStrategy, UnityViewModelResolutionStrategy>();

            // ViewModels
            c.Register<ITemplate10ViewModel, ViewModels.MainPageViewModel>(nameof(Views.MainPage));
            c.Register<ITemplate10ViewModel, ViewModels.DetailPageViewModel>(nameof(Views.DetailPage));
            c.Register<ITemplate10ViewModel, ViewModels.SettingsPageViewModel>(nameof(Views.SettingsPage));
        }
    }
}
