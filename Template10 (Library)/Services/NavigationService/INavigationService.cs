using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Common;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public interface INavigationService
    {
        void Navigate(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null);
        Task<bool> NavigateAsync(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null);

        void Navigate<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible;
        Task<bool> NavigateAsync<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible;

        void GoBack();
        Task<bool> GoBackAsync();

        void GoForward();
        Task<bool> GoForwardAsync();

        void Refresh();
        Task<bool> RefreshAsync();

        void Open(Type page, object parameter = null, string title = null, ViewSizePreference size = ViewSizePreference.UseHalf);
        Task OpenAsync(Type page, object parameter = null, string title = null, ViewSizePreference size = ViewSizePreference.UseHalf);

        void ClearHistory();
        IFrameInfo Frame { get; }
    }

    public interface IFrameInfo
    {
        event TypedEventHandler<IPageInfo> Navigated;
        event TypedEventHandler<IPageInfo> Navigating;

        Task SuspendAsync();
        Task ResumeAsync();

        bool CanGoBack { get; }
        bool CanGoForward { get; }

        IPageInfo Page { get; }

        IList<PageStackEntry> BackStack { get; }
        IList<PageStackEntry> ForwardStack { get; }
    }

    public enum NavMode { New = 0, Back = 1, Forward = 2, Refresh = 3 }

    public interface IPageInfo
    {
        object Content { get; }
        Type Type { get; }
        INavigable DataContext { get; }
        object Parameter { get; }
        NavMode Mode { get; }
        NavigationTransitionInfo TransitionInfo { get; }
    }

    public class PageInfo : IPageInfo
    {
        public PageInfo(NavigationEventArgs e)
        {
            DataContext = (e.Content as Page)?.DataContext as INavigable;
            TransitionInfo = e.NavigationTransitionInfo;
            Mode = (NavMode)(int)e.NavigationMode;
            Parameter = e.Parameter;
            Type = e.SourcePageType;
            Content = e.Content;
            Uri = e.Uri;
        }

        public object Content { get; private set; }
        public INavigable DataContext { get; private set; }
        public NavMode Mode { get; private set; }
        public object Parameter { get; private set; }
        public Type Type { get; private set; }
        public Uri Uri { get; private set; }
        public NavigationTransitionInfo TransitionInfo { get; private set; }
    }

    public class FrameInfo : IFrameInfo
    {
        readonly Frame frame;
        public FrameInfo(Frame frame, Action<NavigationEventArgs> navigatedCallback, Action<NavigatingCancelEventArgs> navigatingCallback)
        {
            this.frame = frame;
            this.frame.Navigated += (s, e) =>
            {
                Page = new PageInfo(e);
                Navigated?.Invoke(this, Page);
                navigatedCallback?.Invoke(e);
            };
            this.frame.Navigating += (s, e) =>
            {
                Navigating?.Invoke(this, Page);
                navigatingCallback?.Invoke(e);
            };
        }

        public event TypedEventHandler<IPageInfo> Navigated;
        public event TypedEventHandler<IPageInfo> Navigating;

        public IList<PageStackEntry> BackStack => frame.BackStack;
        public IList<PageStackEntry> ForwardStack => frame.ForwardStack;

        public bool CanGoBack => frame.CanGoBack;
        public bool CanGoForward => frame.CanGoForward;

        public IPageInfo Page { get; private set; }

        public Task ResumeAsync()
        {
            throw new NotImplementedException();
        }

        public Task SuspendAsync()
        {
            throw new NotImplementedException();
        }
    }
}