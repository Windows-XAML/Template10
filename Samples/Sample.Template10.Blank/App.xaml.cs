using Prism.Ioc;
using Template10.Services;
using Sample.ViewModels;
using Windows.UI.Xaml;
using Template10.Navigation;
using Template10.Ioc;
using Template10;
using Sample.Views;

namespace Sample
{
    sealed partial class App : ApplicationBase 
    {
        public static IPlatformNavigationService NavigationService { get; private set; }

        public App() => InitializeComponent();

        public override void RegisterTypes(IContainerRegistry container)
        {
            container.RegisterView<MainPage, MainPageViewModel>();
        }

        public override void OnInitialized()
        {
            NavigationService = NavigationFactory.Create(Gesture.Back, Gesture.Forward, Gesture.Refresh);
            Window.Current.Content = NavigationService.GetXamlFrame();
            Window.Current.Activate();
        }

        public override void OnStart(StartArgs args)
        {
            NavigationService.NavigateAsync(nameof(MainPage));
        }
    }
}
