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
using System;

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
            var frame = new Frame();
            Window.Current.Content = new ShellPage { Frame = frame };
            Window.Current.Activate();
            _nav = NavigationFactory
                .Create(frame, Guid.Empty.ToString())
                .AttachGestures(Window.Current, Gesture.Back, Gesture.Forward, Gesture.Refresh);
        }

        public override async Task OnStartAsync(IStartArgs args)
        {
            if (args.StartKind == StartKinds.Launch)
            {
                await _nav.NavigateAsync(nameof(MainPage));
            }
        }
    }
}
