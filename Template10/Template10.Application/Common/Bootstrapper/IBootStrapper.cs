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
    //public interface IBootStrapper2
    //{
    //    //static?
    //    ISettings Settings { get; }
    //    IStrategies Strategies { get; }
    //    IStateItems SessionState { get; }

    //    IView[] Views { get; }

    //    Func<SplashScreen, UserControl> SplashFactory { get; set; }

    //    Task OnInitializeAsync();
    //    Task OnStartAsync();
    //    void OnStop();
        
    //    // OnBackstart
    //}

    //public interface IStrategies
    //{
    //    object LifecycleStrategy { get; }
    //}

    //public class BootstrapperX
    //{
    //    public void OnStart()
    //    {
    //    }

    //    public void OnStop()
    //    {
    //        // if closed
    //        // if suspending
    //    }

    //    public void OnResume(e)
    //    {
    //        RestoreNavigation(TimeSpan.FromDays(2), e);
    //    }

    //    public void OnSuspend(e)
    //    {
    //        SaveNavigation(e);
    //    }
    //}

    //public interface IView
    //{
    //    AppViewBackButtonVisibility ShellBackButtonVisibility { get; set; }
    //    INavigationService[] NavigationServices { get; }
    //}

    //public enum ButtonLocations { Shell, App }

    //public interface ISettings
    //{
    //    ButtonLocations ButtonLocation { get; set; }
    //    TimeSpan CacheMaxDuration { get; set; }
    //    bool AutoRestore { get; set; }
    //}

    public interface IBootStrapper
    {
        // don't belong in bootstrapper
        event EventHandler<HandledEventArgs> BackRequested;
        event EventHandler<HandledEventArgs> ForwardRequested;
        event EventHandler ShellBackButtonUpdated;
        bool ShowShellBackButton { get; set; }
        void UpdateShellBackButton();

        // move to nav service
        Dictionary<T, Type> PageKeys<T>() where T : struct, IConvertible;
        INavigationService NavigationServiceFactory(BackButton ignore, ExistingContent exclude, Frame frame = null);
        INavigable ResolveForPage(Page newPage, INavigationService navigationService);

        // strategy
        IWindowLogic WindowLogic { get; }
        ISplashLogic SplashLogic { get; }
        IExtendedSessionService ExtendedSessionService { get; }

        // really is bootstrapper :)
        Func<SplashScreen, UserControl> SplashFactory { get; }
        ApplicationExecutionState PreviousExecutionState { get; }
        IStateItems SessionState { get; }
        TimeSpan CacheMaxDuration { get; set; }
    }
}