using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Services;
using Prism.Unity;
using Sample.Services;
using Sample.ViewModels;
using Sample.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
            container.Register<IDataService, DataService>();
            container.Register<IJumpListService, JumpListService>();
            container.RegisterInstance(new Lazy<IGestureService>(() => GestureService.GetForCurrentView()));
            container.RegisterForNavigation<MainPage, MainPageViewModel>(nameof(MainPage));
            container.RegisterForNavigation<ItemPage, ItemPageViewModel>(nameof(ItemPage));
        }

        public override void OnInitialized()
        {
            NavigationService = Prism.Navigation.NavigationService.Create(Gesture.Back, Gesture.Forward, Gesture.Refresh);
            NavigationService.SetAsWindowContent(Window.Current, activate: true);
        }

        public override void OnStart(StartArgs args)
        {
            if (args.StartCause == StartCauses.JumpListItem && args.Arguments is LaunchActivatedEventArgs e)
            {
                NavigationService.NavigateAsync(new Uri(e.Arguments, UriKind.Relative));
            }
            else
            {
                NavigationService.NavigateAsync(nameof(MainPage));
            }
        }
    }
}
