using System;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Services.Container;
using Template10.Services.Messenger;
using Template10.Navigation;
using Template10.Core;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Template10.BootStrap
{
    public interface IBootStrapper: IBootStrapperShared
    {
        INavigationService NavigationService { get; }

        // publi api

        Task OnInitializeAsync();
        Task OnStartAsync(IStartArgsEx e);
        UIElement CreateRootElement(IStartArgsEx e);
        UIElement CreateSpash(SplashScreen e);
        ISessionState SessionState { get; }
        IMessengerService MessengerService { get; }
        IContainerService ContainerService { get; }
        IDispatcherEx Dispatcher { get; }
    }
}