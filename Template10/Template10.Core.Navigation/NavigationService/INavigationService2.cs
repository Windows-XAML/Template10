using System;
using System.Threading.Tasks;
using Template10.Common;
using Windows.UI.Xaml.Navigation;

namespace Template10.Navigation
{
    public interface INavigationService2 : INavigationService
    {
        Task SaveAsync(bool navigateFrom);
        Task<bool> LoadAsync(bool navigateTo, NavigationMode mode);

        void RaiseForwardRequested(HandledEventArgs args);
        void RaiseBackRequested(HandledEventArgs args);
        void RaiseNavigated(NavigatedEventArgs e);
        void RaiseNavigatingCancels(object parameter, bool suspending, NavigationMode mode, NavigationInfo toInfo, out bool cancel);
        void RaiseBeforeSavingNavigation(out bool cancel);
        void RaiseAfterRestoreSavedNavigation();

        Task SuspendAsync();

        event TypedEventHandler<Type> AfterRestoreSavedNavigation;
    }
}