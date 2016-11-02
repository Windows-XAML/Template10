using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.Dispatcher;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.Navigation
{

    public class FrameFacade : IFrameFacadeInternal
    {
        Frame frame;
        internal FrameFacade(Frame frame)
        {
            frame.Navigated += (s, e) => Navigated?.Invoke(frame, new NavigatedArgs(e));
            frame.Navigating += (s, e) => Navigating?.Invoke(frame, new NavigatingArgs(e));
            Dispatcher = new Dispatcher.DispatcherService(frame.Dispatcher);
        }

        #region IFrameFacadeInternal

        string IFrameFacadeInternal.GetNavigationState() => frame.GetNavigationState();

        void IFrameFacadeInternal.SetNavigationState(string state) => frame.SetNavigationState(state);

        bool IFrameFacadeInternal.CanGoBack => frame.CanGoBack;

        bool IFrameFacadeInternal.GoBack() { frame.GoBack(); return true; }

        void IFrameFacadeInternal.GoBack(NavigationTransitionInfo info) => frame.GoBack();

        bool IFrameFacadeInternal.CanGoForward => frame.CanGoForward;

        bool IFrameFacadeInternal.GoForward() { frame.GoForward(); return true; }

        bool IFrameFacadeInternal.Navigate(Type page) => frame.Navigate(page);

        bool IFrameFacadeInternal.Navigate(Type page, object parameter) => frame.Navigate(page, parameter);

        bool IFrameFacadeInternal.Navigate(Type page, object parameter, NavigationTransitionInfo info) => frame.Navigate(page, parameter, info);

        #endregion

        public event EventHandler<INavigatedArgs> Navigated;

        public event EventHandler<INavigatingArgs> Navigating;

        public IDispatcherService Dispatcher { get; }

        public string Id { get; set; } = string.Empty;

        public object Content => frame?.Content;

        public IReadOnlyList<IStackEntry> ForwardStack => frame.ForwardStack.Select(x => new StackEntry(x)).ToList().AsReadOnly();

        public IReadOnlyList<IStackEntry> BackStack => frame.BackStack.Select(x => new StackEntry(x)).ToList().AsReadOnly();

        public void ClearBackStack() => frame.BackStack.Clear();

    }

}
