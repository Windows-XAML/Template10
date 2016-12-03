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

        internal FrameFacade(Frame frame)
        {
            Frame = frame;
            //frame.Navigated += (s, e) => FacadeNavigatedEventHandler(s, e);
            //frame.Navigating += (s, e) => FacadeNavigatingCancelEventHandler(s, e);

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

        [Obsolete("Use FrameFacade instead. This may be made private in a future version.", false)]
        public Frame Frame { get; }

        public BootStrapper.BackButton BackButtonHandling { get; internal set; }

        public string FrameId { get; set; } = string.Empty;

        [Obsolete("Use NavigationService.Navigate() instead", true)]
        public bool Navigate(Type page, object parameter, NavigationTransitionInfo infoOverride) { throw new NotImplementedException(); }

        [Obsolete("Use NavigationService.NavigationState instead", true)]
        public void SetNavigationState(string state) { throw new NotImplementedException(); }

        [Obsolete("Use NavigationService.NavigationState instead", true)]
        public string GetNavigationState() { throw new NotImplementedException(); }

        public int BackStackDepth => Frame.BackStackDepth;

        public bool CanGoBack => Frame.CanGoBack;

        [Obsolete("No longer valid.", true)]
        public NavigationMode NavigationModeHint = NavigationMode.New;

        [Obsolete("Use NavigationService.GoBack()", true)]
        public void GoBack(NavigationTransitionInfo infoOverride = null) { throw new NotImplementedException(); }

        [Obsolete("Use NavigationService.Refresh()", true)]
        public void Refresh() { throw new NotImplementedException(); }

        [Obsolete("Use NavigationService.Refresh()", true)]
        public void Refresh(object param) { throw new NotImplementedException(); }

        public bool CanGoForward => Frame.CanGoForward;

        [Obsolete("Use NavigationService.GoForward()", true)]
        public void GoForward() { throw new NotImplementedException(); }

        public object Content => Frame.Content;

        [Obsolete("Use NavigationService.LastNavigationType", true)]
        public Type CurrentPageType { get; internal set; }

        [Obsolete("Use NavigationService.LastNavigationParameter", true)]
        public object CurrentPageParam { get; internal set; }

        [Obsolete("Use NavigationService.SerializationService", false)]
        public ISerializationService SerializationService { get; set; } = Services.SerializationService.SerializationService.Json;

        public object GetValue(DependencyProperty dp) => Frame.GetValue(dp);

        public void SetValue(DependencyProperty dp, object value) { Frame.SetValue(dp, value); }

        public void ClearValue(DependencyProperty dp) { Frame.ClearValue(dp); }

        #endregion

        public event EventHandler<NavigatedEventArgs> Navigated;
        internal void RaiseNavigated(NavigatedEventArgs e) => Navigated?.Invoke(this, e);

        public event EventHandler<NavigatingEventArgs> Navigating;
        internal void RaiseNavigating(NavigatingEventArgs e) => Navigating?.Invoke(this, e);
    }

}
