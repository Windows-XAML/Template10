using System;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Services.Container;
using Template10.Services.Messenger;
using Template10.Navigation;
using Template10.Core;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Template10.Core;

namespace Template10
{
    public interface IBootStrapper: IBootStrapperShared
    {
        INavigationService NavigationService { get; }

        // publi api

        Task OnStartAsync(IStartArgsEx e);
        Task<UIElement> CreateRootElement(IStartArgsEx e);
        Task<UIElement> CreateSpashAsync(SplashScreen e);
    }
}