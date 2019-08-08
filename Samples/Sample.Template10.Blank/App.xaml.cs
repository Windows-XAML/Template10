using Prism.Ioc;
using Template10.Services;
using Sample.ViewModels;
using Windows.UI.Xaml;
using Template10.Navigation;
using Template10.Ioc;
using Template10;
using Sample.Views;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace Sample
{
    sealed partial class App : ApplicationBase
    {
        private INavigationService _nav;

        public App() => InitializeComponent();

        public override void RegisterTypes(IContainerRegistry container)
        {
            container.RegisterView<MainPage, MainPageViewModel>();
        }

        public override void OnInitialized()
        {
            _nav = NavigationFactory
                .Create(new Frame())
                .ActivateWindow(Window.Current)
                .AttachGestures(Gesture.Back, Gesture.Forward, Gesture.Refresh);
        }

        public override async Task OnStartAsync(IStartArgs args)
        {
            await _nav.NavigateAsync(nameof(MainPage));
        }
    }
}
