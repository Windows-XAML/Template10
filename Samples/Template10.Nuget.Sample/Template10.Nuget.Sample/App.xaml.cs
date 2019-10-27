using Template10.Nuget.Sample.ViewModels;
using Template10.Nuget.Sample.Views;
using Prism.Ioc;
using System;
using System.Threading.Tasks;
using Template10.Ioc;
using Template10.Navigation;
using Template10.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Nuget.Sample
{
    sealed partial class App : Template10.ApplicationBase
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
