using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Services.ExtendedSessionService;
using Template10.Services.NavigationService;
using Template10.Services.StateService;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Template10.Common
{
    public interface IBootStrapper2
    {
        ISettings Settings { get; }
        IStateItems SessionState { get; }

        IView[] Views { get; }
        Func<SplashScreen, UserControl> SplashFactory { get; set; }

        Task OnInitializeAsync();
        Task OnStartAsync();
    }

    public interface IView
    {
        AppViewBackButtonVisibility ShellBackButtonVisibility { get; set; }
        INavigationService[] NavigationServices { get; }
    }

    public enum ButtonLocations { Shell, App }

    public interface ISettings
    {
        ButtonLocations ButtonLocation { get; set; }
        TimeSpan CacheMaxDuration { get; set; }
        bool AutoRestore { get; set; }
    }

    public interface IBootStrapper
    {
        event EventHandler<HandledEventArgs> BackRequested;

        event EventHandler<HandledEventArgs> ForwardRequested;

        event EventHandler ShellBackButtonUpdated;

        Func<SplashScreen, UserControl> SplashFactory { get; }

        IWindowLogic WindowLogic { get; }

        ISplashLogic SplashLogic { get; }

        IExtendedSessionService ExtendedSessionService { get; }
        ApplicationExecutionState PreviousExecutionState { get; }
        IStateItems SessionState { get; }
        bool ShowShellBackButton { get; set; }
        TimeSpan CacheMaxDuration { get; set; }

        void UpdateShellBackButton();
        Dictionary<T, Type> PageKeys<T>() where T : struct, IConvertible;
        INavigable ResolveForPage(Page newPage, INavigationService navigationService);
        INavigationService NavigationServiceFactory(BackButton ignore, ExistingContent exclude, Frame frame = null);
    }
}