using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Template10.Services;
using Prism.Unity;
using Sample.ViewModels;
using Windows.UI.Xaml;
using Template10.Navigation;
using Template10.Ioc;
using Template10;

namespace Sample
{
    sealed partial class App : PrismApplication
    {
        public static IPlatformNavigationService NavigationService { get; private set; }

        public App()
        {
            InitializeComponent();
        }

        public override void RegisterTypes(IContainerRegistry container)
        {
            container.RegisterForNavigation<Views.MainPage, MainPageViewModel>(nameof(Views.MainPage));
        }

        public override void OnInitialized()
        {
            NavigationService = NavigationFactory.Create(Gesture.Back, Gesture.Forward, Gesture.Refresh);
            NavigationService.SetAsWindowContent(Window.Current, activate: true);
        }

        public override void OnStart(StartArgs args)
        {
            NavigationService.NavigateAsync(nameof(Views.MainPage));
        }
    }
}
