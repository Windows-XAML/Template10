using System;
using System.Collections.Generic;
using Template10.Services.ExtendedSessionService;
using Template10.Services.NavigationService;
using Template10.Services.StateService;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;

namespace Template10.Common
{
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