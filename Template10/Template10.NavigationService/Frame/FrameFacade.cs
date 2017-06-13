using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Template10.Common;
using Template10.Portable.Navigation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Navigation-Service
    public partial class FrameFacade : IFrameFacadeInternal, IFrameFacade
    {
        #region Debug

        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            LoggingService.LoggingService.WriteLine(text, severity, caller: $"{nameof(FrameFacade)}.{caller}");

        #endregion

        // [Obsolete("Internal use only")]
        internal FrameFacade(Frame frame, INavigationService navigationService)
        {
            (this as IFrameFacadeInternal).Frame = frame;
            (this as IFrameFacadeInternal).NavigationService = navigationService;

            // setup animations
            var t = new NavigationThemeTransition
            {
                DefaultNavigationTransitionInfo = new EntranceNavigationTransitionInfo()
            };
            (this as IFrameFacadeInternal).Frame.ContentTransitions = new TransitionCollection { };
            (this as IFrameFacadeInternal).Frame.ContentTransitions.Add(t);
        }

        public string FrameId { get; set; } = string.Empty;

        public object Content => (this as IFrameFacadeInternal).Frame.Content;

        public object GetValue(DependencyProperty dp) => (this as IFrameFacadeInternal).Frame.GetValue(dp);

        public void SetValue(DependencyProperty dp, object value) { (this as IFrameFacadeInternal).Frame.SetValue(dp, value); }

        public void ClearValue(DependencyProperty dp) { (this as IFrameFacadeInternal).Frame.ClearValue(dp); }

        public bool CanGoBack => (this as IFrameFacadeInternal).Frame.CanGoBack;

        public bool CanGoForward => (this as IFrameFacadeInternal).Frame.CanGoForward;

        public void ClearCache(bool removeCachedPagesInBackStack = false)
        {
            DebugWrite($"Frame: {FrameId}");

            var frame = (this as IFrameFacadeInternal).Frame;

            int currentSize = frame.CacheSize;
            try
            {
                if (removeCachedPagesInBackStack)
                {
                    frame.CacheSize = 0;
                }
                else
                {
                    if (frame.BackStackDepth == 0)
                    {
                        frame.CacheSize = 1;
                    }
                    else
                    {
                        frame.CacheSize = frame.BackStackDepth;
                    }
                }
            }
            catch
            {
                // Ignore exceptions here
            }
            finally
            {
                frame.CacheSize = currentSize;
            }
        }

        internal bool Navigate(Type page, object parameter) => (this as IFrameFacadeInternal).Frame.Navigate(page, parameter);

        public IList<PageStackEntry> BackStack => (this as IFrameFacadeInternal).Frame.BackStack;

        public IList<PageStackEntry> ForwardStack => (this as IFrameFacadeInternal).Frame.ForwardStack;

    }

    public partial class FrameFacade : IFrameFacadeInternal
    {
        // Obsolete properties/methods

        [Obsolete("This may be made private in a future version.", false)]
        public void GoBack() => (this as IFrameFacadeInternal).GoBack();

        [Obsolete("This may be made private in a future version.", false)]
        public void GoForward() => (this as IFrameFacadeInternal).GoForward();

        [Obsolete("Use FrameFacade.BackStack.Count. THis will be deleted in future versions.", false)]
        public int BackStackDepth => (this as IFrameFacadeInternal).Frame.BackStack.Count;

        [Obsolete("Use NavigationService.BackButtonHandling This may be made private in a future version.", false)]
        public Common.BackButton BackButtonHandling
        {
            get { return NavigationService.BackButtonHandling; }
            internal set { NavigationService.BackButtonHandling = value; }
        }

        [Obsolete("Use NavigationService.Suspension.GetFrameState(). This may be made private in a future version.", false)]
        private SettingsService.ISettingsService FrameStateSettingsService() => this.NavigationService.Suspension.GetFrameState();

        [Obsolete("Use NavigationService.Suspension.GetFrameState().Write(). This may be made private in a future version.", false)]
        public void SetFrameState(string key, string value) => this.NavigationService.Suspension.GetFrameState().Write(key, value);

        [Obsolete("Use NavigationService.Suspension.GetFrameState().Read(). This may be made private in a future version.", false)]
        public string GetFrameState(string key, string otherwise) => this.NavigationService.Suspension.GetFrameState().Read(key, otherwise);

        [Obsolete("Use NavigationService.Suspension.ClearFrameState(). This may be made private in a future version.", false)]
        public void ClearFrameState() => this.NavigationService.Suspension.ClearFrameState();

        [Obsolete("Use NavigationService.Suspension.GetPageState(). This may be made private in a future version.", false)]
        public SettingsService.ISettingsService PageStateSettingsService(Type type) => this.NavigationService.Suspension.GetPageState(type, BackStack.Count);

        [Obsolete("Use NavigationService.Suspension.GetPageState(). This may be made private in a future version.", false)]
        public SettingsService.ISettingsService PageStateSettingsService(string key) => this.NavigationService.Suspension.GetPageState(key);

        [Obsolete("Use NavigationService.Suspension.ClearPageState(). This may be made private in a future version.", false)]
        public void ClearPageState(Type type) => this.NavigationService.Suspension.ClearPageState(type, BackStack.Count);

        [Obsolete("This may be made private in a future version.", false)]
        public INavigationService NavigationService => (this as IFrameFacadeInternal).NavigationService;

        [Obsolete("This will be made private in a future version.", true)]
        public Frame Frame => (this as IFrameFacadeInternal).Frame;

        [Obsolete("This may be made private in a future version.", true)]
        public NavigationMode NavigationModeHint { get; } = NavigationMode.New;

        [Obsolete("Use NavigationService.Refresh(). This may be made private in a future version.", false)]
        public void Refresh() => NavigationService.Refresh();

        [Obsolete("Use NavigationService.Refresh(). This may be made private in a future version.", false)]
        public void Refresh(object param) => NavigationService.Refresh(param);

        [Obsolete("Use NavigationService.LastNavigationType. This may be made private in a future version.", false)]
        public Type CurrentPageType => NavigationService.CurrentPageType;

        [Obsolete("Use NavigationService.LastNavigationParameter. This may be made private in a future version.", false)]
        public object CurrentPageParam => NavigationService.CurrentPageParam;

        [Obsolete("Use NavigationService.SerializationService. This may be made private in a future version.", false)]
        public SerializationService.ISerializationService SerializationService => NavigationService.SerializationService;

        [Obsolete("Use NavigationService.Navigated. This may be made private in a future version.", false)]
        public event EventHandler<NavigatedEventArgs> Navigated;

        [Obsolete("Use NavigationService.Navigating. This may be made private in a future version.", false)]
        public event EventHandler<NavigatingEventArgs> Navigating;

        [Obsolete("Use NavigationService.BackRequested. This may be made private in a future version.", false)]
        public event EventHandler<HandledEventArgs> BackRequested;

        [Obsolete("Use NavigationService.ForwardRequested. This may be made private in a future version.", false)]
        public event EventHandler<HandledEventArgs> ForwardRequested;

    }
    public partial class FrameFacade : IFrameFacadeInternal
    {
        // Internal properties/methods

        void IFrameFacadeInternal.SetNavigationState(string state) => (this as IFrameFacadeInternal).Frame.SetNavigationState(state);

        string IFrameFacadeInternal.GetNavigationState() => (this as IFrameFacadeInternal).Frame.GetNavigationState();

        void IFrameFacadeInternal.GoBack(NavigationTransitionInfo infoOverride) => (this as IFrameFacadeInternal).Frame.GoBack();

        void IFrameFacadeInternal.GoForward() => (this as IFrameFacadeInternal).Frame.GoForward();

        bool IFrameFacadeInternal.Navigate(Type page, object parameter, NavigationTransitionInfo info) => (this as IFrameFacadeInternal).Frame.Navigate(page, parameter, info);

        void IFrameFacadeInternal.GoBack() => (this as IFrameFacadeInternal).Frame.GoBack();

        Frame IFrameFacadeInternal.Frame { get; set; }

        INavigationService IFrameFacadeInternal.NavigationService { get; set; }

        void IFrameFacadeInternal.RaiseNavigated(NavigatedEventArgs e) => Navigated?.Invoke(this, e);

        void IFrameFacadeInternal.RaiseNavigating(NavigatingEventArgs e) => Navigating?.Invoke(this, e);

        void IFrameFacadeInternal.RaiseBackRequested(HandledEventArgs args) => BackRequested?.Invoke(this, args);

        void IFrameFacadeInternal.RaiseForwardRequested(HandledEventArgs args) => ForwardRequested?.Invoke(this, args);

    }
}
