using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Unity;
using Sample.ViewModels;
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
        public App()
        {
            InitializeComponent();
        }

        public override void RegisterTypes(IContainerRegistry container)
        {
            container.RegisterForNavigation<MainPage, MainPageViewModel>(nameof(MainPage));
        }

        public override void OnInitialized()
        {
            var frame = new Frame();
            var navigationService = NavigationService
                .Create(frame, Gestures.Back, Gestures.Forward, Gestures.Refresh);
            Window.Current.Content = frame;
            Window.Current.Activate();
        }

        public override void OnStart(StartArgs args)
        {
        }
    }
}
