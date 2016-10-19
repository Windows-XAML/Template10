using System;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Navigation
{
    public class NavigationService : INavigationService
    {
        private Windows.UI.Xaml.Controls.Frame Frame;

        public NavigationService(Windows.UI.Xaml.Controls.Frame frame)
        {
            Frame = frame;
        }

        public INavigationItems BackStack { get; }

        public bool CanGoBack { get; }

        public bool CanGoForward { get; }

        public INavigationItem Current
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public INavigationItems ForwardStack
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        // private?
        public Windows.UI.Xaml.Controls.Page Page => Frame.Content as Windows.UI.Xaml.Controls.Page;

        public object ViewModel => Page?.DataContext;

        public INavigationState NavigationState { get; }

        public Suspension.ISuspensionState SuspensionState { get; }

        public void GoBack(NavigationTransitionInfo infoOverride = null)
        {
            throw new NotImplementedException();
        }

        public void GoForward()
        {
            throw new NotImplementedException();
        }

        public Task<bool> NavigateAsync(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> NavigateAsync<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible
        {
            throw new NotImplementedException();
        }

        public Task<LifetimeService.ILifetimeService> OpenAsync(Type page, object parameter = null, string title = null, ViewSizePreference size = ViewSizePreference.UseHalf)
        {
            throw new NotImplementedException();
        }
    }
}
