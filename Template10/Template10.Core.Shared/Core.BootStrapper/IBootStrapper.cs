using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.BootStrap;
using Template10.Services.Container;
using Template10.Services.Messenger;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Core
{
    public interface IBootStrapper
    {
        Task OnInitializeAsync();
        Task OnStartAsync(IStartArgsEx e);
        void OnClose(ClosedEventArgs e);
        UIElement CreateRootElement(IStartArgsEx e);
        UIElement CreateSpash(SplashScreen e);
        IContainerService CreateDependecyContainer();
        void RegisterCustomDependencies();

        DataTemplate BusyIndicatorTemplate { get; set; }
        DataTemplate SplashScreenTemplate { get; set; }
        DataTemplate NetworkRequiredTemplate { get; set; }
    }
}
