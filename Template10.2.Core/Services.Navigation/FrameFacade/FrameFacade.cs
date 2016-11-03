using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.Dispatcher;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.Navigation
{

    public class FrameFacade : IFrameFacadeInternal
    {
        Frame xamlFrame;
        internal FrameFacade(Frame frame)
        {
            frame.Navigated += (s, e) => Navigated?.Invoke(frame, new NavigatedArgs(e));
            frame.Navigating += (s, e) => Navigating?.Invoke(frame, new NavigatingArgs(e));
            Dispatcher = new Dispatcher.DispatcherService(frame.Dispatcher);
        }

        #region explicit IFrameFacadeInternal

        bool IFrameFacadeInternal.CanGoBack => xamlFrame.CanGoBack;

        bool IFrameFacadeInternal.GoBack() { xamlFrame.GoBack(); return true; }

        void IFrameFacadeInternal.GoBack(NavigationTransitionInfo info) => xamlFrame.GoBack();

        bool IFrameFacadeInternal.CanGoForward => xamlFrame.CanGoForward;

        bool IFrameFacadeInternal.GoForward() { xamlFrame.GoForward(); return true; }

        bool IFrameFacadeInternal.Navigate(Type page) => xamlFrame.Navigate(page);

        bool IFrameFacadeInternal.Navigate(Type page, IPropertySet parameter) => xamlFrame.Navigate(page, parameter);

        bool IFrameFacadeInternal.Navigate(Type page, IPropertySet parameter, NavigationTransitionInfo info) => xamlFrame.Navigate(page, parameter, info);

        #endregion

        public event EventHandler<INavigatedArgs> Navigated;

        public event EventHandler<INavigatingArgs> Navigating;

        public IDispatcherService Dispatcher { get; }

        public string Id { get; set; } = string.Empty;

        public object Content => xamlFrame?.Content;

        public IReadOnlyList<IStackEntry> ForwardStack => xamlFrame.ForwardStack.Select(x => new StackEntry(x)).ToList().AsReadOnly();

        public IReadOnlyList<IStackEntry> BackStack => xamlFrame.BackStack.Select(x => new StackEntry(x)).ToList().AsReadOnly();

        public void ClearBackStack() => xamlFrame.BackStack.Clear();

        public string GetNavigationState() => xamlFrame.GetNavigationState();

        public void SetNavigationState(string state) => xamlFrame.SetNavigationState(state);
    }

}
