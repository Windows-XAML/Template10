using System;
using Template10.Navigation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Prism.Ioc;
using System.Threading.Tasks;
using Template10.Ioc;
using Template10.Services;
using Template10.NuGet.FileActivationSample.ViewModels;
using Template10.NuGet.FileActivationSample.Views;
using Windows.ApplicationModel.Activation;
using System.Collections.Generic;
using Windows.Storage;
using System.Linq;

namespace Template10.NuGet.FileActivationSample
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Template10.ApplicationBase
    {
        private INavigationService _nav;

        public App() => InitializeComponent();

        public override void RegisterTypes(IContainerRegistry container)
        {
            container.RegisterView<MainPage, MainPageViewModel>();
            container.RegisterView<MarkdownPage, MarkdownPageViewModel>();
            container.Register<IDialogService, DialogService>();
        }

        public override void OnInitialized()
        {
            var shell = (ShellPage)Container.Resolve(typeof(ShellPage));
            var frame = shell.Frame = new Frame();
            Window.Current.Content = shell;
            Window.Current.Activate();
            _nav = NavigationFactory
                .Create(frame, Guid.Empty.ToString())
                .AttachGestures(Window.Current, Gesture.Back, Gesture.Forward, Gesture.Refresh);
        }

        public override async Task OnStartAsync(IStartArgs args)
        {
            switch (args.StartCause)
            {
                case StartCauses.File when (args.Arguments is FileActivatedEventArgs fileArgs):
                    var navParams = new NavigationParameters(("file", fileArgs.Files.First()));
                    await _nav.NavigateAsync(nameof(MarkdownPage), navParams);
                    break;
                default:
                    await _nav.NavigateAsync(nameof(MainPage));
                    break;
            }
        }
    }
}
