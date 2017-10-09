using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.BootStrap;
using Template10.Navigation;
using Template10.Services.Dependency;
using Template10.Services.Messenger;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Core
{
    public interface IBootStrapperDependecyInjection
    {
        IDependencyService CreateDependecyService();
        void RegisterCustomDependencies(IDependencyService dependencyService);
    }

    public interface IBootStrapperStartup
    {
        Task OnInitializeAsync();
        UIElement CreateRootElement(IStartArgsEx e);
        Task OnStartAsync(IStartArgsEx e, INavigationService navService, ISessionState sessionState);
    }

    public interface IBootStrapperPopup
    {
        IEnumerable<Popups.IPopupItem> Popups { get; }
    }
}