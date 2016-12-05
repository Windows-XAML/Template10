using System;
using System.Collections.Generic;
using Template10.Common;
using Template10.Services.SettingsService;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Template10.Services.SerializationService;
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
            Frame = frame;
            this.NavigationService = navigationService;

            // setup animations
            var t = new NavigationThemeTransition
            {
                DefaultNavigationTransitionInfo = new EntranceNavigationTransitionInfo()
            };
            Frame.ContentTransitions = new TransitionCollection { };
            Frame.ContentTransitions.Add(t);
        }

        public event EventHandler<HandledEventArgs> BackRequested;
        public void RaiseBackRequested(HandledEventArgs args) => BackRequested?.Invoke(this, args);

        public event EventHandler<HandledEventArgs> ForwardRequested;
        public void RaiseForwardRequested(HandledEventArgs args) => ForwardRequested?.Invoke(this, args);

        #region state

        private string GetFrameStateKey() => string.Format("{0}-PageState", FrameId);

        private ISettingsService FrameStateSettingsService()
        {
            return SettingsService.SettingsService.Create(SettingsStrategies.Local, GetFrameStateKey(), true);
        }

        public void SetFrameState(string key, string value)
        {
            FrameStateSettingsService().Write(key, value);
        }

        public string GetFrameState(string key, string otherwise)
        {
            return FrameStateSettingsService().Read(key, otherwise);
        }

        public void ClearFrameState()
        {
            FrameStateSettingsService().Clear();
        }

        private string GetPageStateKey(string frameId, Type type, int backStackDepth) => $"{frameId}-{type}-{backStackDepth}";

        public ISettingsService PageStateSettingsService(Type type)
        {
            return FrameStateSettingsService().Open(GetPageStateKey(FrameId, type, BackStackDepth), true);
        }

        public ISettingsService PageStateSettingsService(string key)
        {
            return FrameStateSettingsService().Open(key, true);
        }

        public void ClearPageState(Type type)
        {
            this.FrameStateSettingsService().Remove(GetPageStateKey(FrameId, type, BackStackDepth));
        }

        #endregion

        #region frame facade

        public BootStrapper.BackButton BackButtonHandling { get; internal set; }

        public string FrameId { get; set; } = string.Empty;

        public int BackStackDepth => Frame.BackStackDepth;

        public bool CanGoBack => Frame.CanGoBack;

        public bool CanGoForward => Frame.CanGoForward;

        public object Content => Frame.Content;

        public object GetValue(DependencyProperty dp) => Frame.GetValue(dp);

        public void SetValue(DependencyProperty dp, object value) { Frame.SetValue(dp, value); }

        public void ClearValue(DependencyProperty dp) { Frame.ClearValue(dp); }

        // Obsolete properties/methods

        [Obsolete("This may be made private in a future version.")]
        public INavigationService NavigationService { get; }

        [Obsolete("Use FrameFacade instead. This may be made private in a future version.", false)]
        public Frame Frame { get; }

        [Obsolete("Use NavigationService.Navigate() instead. This may be made private in a future version.", true)]
        public bool Navigate(Type page, object parameter, NavigationTransitionInfo infoOverride) { throw new NotImplementedException("FrameFacade.Navigate is obsolete; use NavigationService.Navigate()."); }

        [Obsolete("Use NavigationService.NavigationState instead. This may be made private in a future version.", true)]
        public void SetNavigationState(string state) { throw new NotImplementedException(); }

        [Obsolete("Use NavigationService.NavigationState instead. This may be made private in a future version.", true)]
        public string GetNavigationState() { throw new NotImplementedException(); }

        [Obsolete("This may be made private in a future version.", true)]
        public NavigationMode NavigationModeHint = NavigationMode.New;

        [Obsolete("Use NavigationService.GoBack(). This may be made private in a future version.", false)]
        public void GoBack(NavigationTransitionInfo infoOverride = null) => NavigationService.GoBack();

        [Obsolete("Use NavigationService.Refresh(). This may be made private in a future version.", false)]
        public void Refresh() => NavigationService.Refresh();

        [Obsolete("Use NavigationService.Refresh(). This may be made private in a future version.", false)]
        public void Refresh(object param) => NavigationService.Refresh(param);

        [Obsolete("Use NavigationService.GoForward(). This may be made private in a future version.", false)]
        public void GoForward() => NavigationService.GoForward();

        [Obsolete("Use NavigationService.LastNavigationType. This may be made private in a future version.", false)]
        public Type CurrentPageType => NavigationService.CurrentPageType;

        [Obsolete("Use NavigationService.LastNavigationParameter. This may be made private in a future version.", false)]
        public object CurrentPageParam => NavigationService.CurrentPageParam;

        [Obsolete("Use NavigationService.SerializationService. This may be made private in a future version.", false)]
        public ISerializationService SerializationService => NavigationService.SerializationService;

        #endregion

        public event EventHandler<NavigatedEventArgs> Navigated;
        internal void RaiseNavigated(NavigatedEventArgs e) => Navigated?.Invoke(this, e);

        public event EventHandler<NavigatingEventArgs> Navigating;
        internal void RaiseNavigating(NavigatingEventArgs e) => Navigating?.Invoke(this, e);
    }

}
