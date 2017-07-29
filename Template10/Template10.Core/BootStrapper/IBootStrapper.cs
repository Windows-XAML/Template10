using System;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Services.Container;
using Template10.Services.Messenger;
using Template10.Navigation;
using Template10.StartArgs;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Template10.Core;

namespace Template10
{
    public interface IBootStrapper: IBootStrapperShared
    {
        IDispatcherEx Dispatcher { get; }
        INavigationService NavigationService { get; }

        // publi api

        Task OnStartAsync(ITemplate10StartArgs e);
        Task<UIElement> CreateRootElement(ITemplate10StartArgs e);
        Task<UIElement> CreateSpashAsync(SplashScreen e);
    }
}