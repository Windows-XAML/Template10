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

        Task SuspendAsync();

        event TypedEventHandler<Type> AfterRestoreSavedNavigation;
    }
}