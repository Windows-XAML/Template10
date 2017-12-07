using System;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Strategies;
using Windows.UI.Xaml.Navigation;

namespace Template10.Navigation
{
    public interface INavigationService2 : INavigationService
    {
        Task SaveAsync(bool navigateFrom);
        Task<bool> LoadAsync(bool navigateTo, NavigationMode mode);

        void RaiseForwardRequested(HandledEventArgsEx args);
        void RaiseBackRequested(HandledEventArgsEx args);
        void RaiseNavigated(NavigatedEventArgs e);
        void RaiseNavigatingCancels(object parameter, bool suspending, NavigationMode mode, NavigationInfo toInfo, out ContinueResult result);
        void RaiseBeforeSavingNavigation(out bool cancel);
        void RaiseAfterRestoreSavedNavigation();

        Task SuspendAsync();

        event TypedEventHandler<Type> AfterRestoreSavedNavigation;
    }
}