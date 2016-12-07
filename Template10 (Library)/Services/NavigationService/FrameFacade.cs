using System;
using System.Collections.Generic;
using Template10.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using System.Runtime.CompilerServices;

namespace Template10.Services.NavigationService
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-NavigationService
    public class FrameFacade
    {
        #region Debug

        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            LoggingService.LoggingService.WriteLine(text, severity, caller: $"{nameof(FrameFacade)}.{caller}");

        #endregion

        internal FrameFacade(Frame frame, INavigationService navigationService)
        {
            _Frame = frame;
            this.NavigationService = navigationService;

            // setup animations
            var t = new NavigationThemeTransition
            {
                DefaultNavigationTransitionInfo = new EntranceNavigationTransitionInfo()
            };
            _Frame.ContentTransitions = new TransitionCollection { };
            _Frame.ContentTransitions.Add(t);
        }

        public string FrameId { get; set; } = string.Empty;

        public int BackStackDepth => _Frame.BackStackDepth;

        public object Content => _Frame.Content;

        public object GetValue(DependencyProperty dp) => _Frame.GetValue(dp);

        public void SetValue(DependencyProperty dp, object value) { _Frame.SetValue(dp, value); }

        public void ClearValue(DependencyProperty dp) { _Frame.ClearValue(dp); }

        public void SetNavigationState(string state) => _Frame.SetNavigationState(state);

        public string GetNavigationState() => _Frame.GetNavigationState();

        public bool CanGoBack => _Frame.CanGoBack;

        public bool CanGoForward => _Frame.CanGoForward;

        public void GoBack(NavigationTransitionInfo infoOverride = null) => _Frame.GoBack();

        public void GoForward() => _Frame.GoForward();

        public void ClearCache(bool removeCachedPagesInBackStack = false)
        {
            DebugWrite($"Frame: {FrameId}");

            int currentSize = _Frame.CacheSize;

            if (removeCachedPagesInBackStack)
            {
                _Frame.CacheSize = 0;
            }
            else
            {
                if (_Frame.BackStackDepth == 0)
                {
                    _Frame.CacheSize = 1;
                }
                else
                {
                    _Frame.CacheSize = _Frame.BackStackDepth;
                }
            }

            _Frame.CacheSize = currentSize;
        }

        internal bool Navigate(Type page, object parameter, NavigationTransitionInfo info)
        {
            return _Frame.Navigate(page, parameter, info);
        }

        internal bool Navigate(Type page, object parameter)
        {
            return _Frame.Navigate(page, parameter);
        }

        internal IList<PageStackEntry> BackStack => _Frame.BackStack;

        internal IList<PageStackEntry> ForwardStack => _Frame.ForwardStack;

        // Obsolete properties/methods

        [Obsolete("Use NavigationService.BackButtonHandling This may be made private in a future version.", false)]
        public BootStrapper.BackButton BackButtonHandling
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
        public SettingsService.ISettingsService PageStateSettingsService(Type type) => this.NavigationService.Suspension.GetPageState(type, BackStackDepth);

        [Obsolete("Use NavigationService.Suspension.GetPageState(). This may be made private in a future version.", false)]
        public SettingsService.ISettingsService PageStateSettingsService(string key) => this.NavigationService.Suspension.GetPageState(key);

        [Obsolete("Use NavigationService.Suspension.ClearPageState(). This may be made private in a future version.", false)]
        public void ClearPageState(Type type) => this.NavigationService.Suspension.ClearPageState(type, BackStackDepth);

        [Obsolete("This may be made private in a future version.", false)]
        public INavigationService NavigationService { get; }

        public Frame _Frame;
        [Obsolete("Use FrameFacade instead. This may be made private in a future version.", false)]
        public Frame Frame { get { return _Frame; } set { _Frame = value; } }

        [Obsolete("This may be made private in a future version.", true)]
        public NavigationMode NavigationModeHint = NavigationMode.New;

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
        [Obsolete]
        internal void RaiseNavigated(NavigatedEventArgs e) => Navigated?.Invoke(this, e);

        [Obsolete("Use NavigationService.Navigating. This may be made private in a future version.", false)]
        public event EventHandler<NavigatingEventArgs> Navigating;
        [Obsolete]
        internal void RaiseNavigating(NavigatingEventArgs e) => Navigating?.Invoke(this, e);

        [Obsolete("Use NavigationService.BackRequested. This may be made private in a future version.", false)]
        public event EventHandler<HandledEventArgs> BackRequested;
        [Obsolete]
        internal void RaiseBackRequested(HandledEventArgs args) => BackRequested?.Invoke(this, args);

        [Obsolete("Use NavigationService.ForwardRequested. This may be made private in a future version.", false)]
        public event EventHandler<HandledEventArgs> ForwardRequested;
        [Obsolete]
        internal void RaiseForwardRequested(HandledEventArgs args) => ForwardRequested?.Invoke(this, args);
    }

}
