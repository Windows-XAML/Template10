using System;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Dispatcher;
using Template10.Services.Container;
using Template10.Services.Messenger;
using Template10.Services.NavigationService;
using Template10.StartArgs;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Template10
{
    public interface IBootStrapper: IBootStrapperShared
    {
        IMessengerService MessengerService { get; }
        IContainerService ContainerService { get; }
        ITemplate10Dispatcher Dispatcher { get; }
        INavigationService NavigationService { get; }

        Task OnStartAsync(ITemplate10StartArgs e);
        Task<UIElement> CreateRootElement(ITemplate10StartArgs e);
        Task<UIElement> CreateSpashAsync(SplashScreen e);
    }
}