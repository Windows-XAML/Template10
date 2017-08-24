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
    public interface IBootStrapper : IBootStrapperShared
    {
        Task OnInitializeAsync();
        Task OnStartAsync(IStartArgsEx e);
        void OnClose(ClosedEventArgs e);
        UIElement CreateRootElement(IStartArgsEx e);
        UIElement CreateSpash(SplashScreen e);
        IContainerService CreateDependecyContainer();
        void RegisterCustomDependencies();
    }
}