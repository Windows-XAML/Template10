using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using T102.ViewModelLocator.Strategies;
using Template10.BootStrap;
using Template10.Core;
using Template10.Mvvm;
using Template10.Services.Container;
using Template10.Services.Gesture;
using Template10.Services.Logging;
using Template10.Services.Messenger;
using Template10.Services.Resources;
using Template10.Services.Serialization;
using Template10.Strategies;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace T102.ViewModelLocator
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
            c.Register<IKeyboardService, KeyboardService>();
            c.Register<IGestureService, GestureService>();
            c.Register<IResourceService, ResourceService>();

            // strategies
            c.Register<IBootStrapperShared>(this);
            c.Register<IBootStrapperStrategy, DefaultBootStrapperStrategy>();
            c.Register<ILifecycleStrategy, DefaultLifecycleStrategy>();
            c.Register<IStateStrategy, DefaultStateStrategy>();
            c.Register<ITitleBarStrategy, DefaultTitleBarStrategy>();
            c.Register<IExtendedSessionStrategy, DefaultExtendedSessionStrategy>();
            c.Register<IViewModelActionStrategy, DefaultViewModelActionStrategy>();
            c.Register<IViewModelResolutionStrategy, UnityViewModelResolutionStrategy>();

            // ViewModels
            c.Register<ViewModelBase, ViewModels.MainPageViewModel>(nameof(Views.MainPage));
            c.Register<ViewModelBase, ViewModels.DetailPageViewModel>(nameof(Views.DetailPage));
            c.Register<ViewModelBase, ViewModels.SettingsPageViewModel>(nameof(Views.SettingsPage));
        }
    }
}
