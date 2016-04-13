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

        internal FrameFacade(NavigationService navigationService, Frame frame)
        {
            NavigationService = navigationService;
            Frame = frame;
            frame.Navigated += (s, e) => FacadeNavigatedEventHandler(s, e);
            frame.Navigating += (s, e) => FacadeNavigatingCancelEventHandler(s, e);

            // setup animations
            var t = new NavigationThemeTransition
            {
                DefaultNavigationTransitionInfo = new EntranceNavigationTransitionInfo()
            };
            Frame.ContentTransitions = new TransitionCollection { };
            Frame.ContentTransitions.Add(t);
        }

        public event EventHandler<HandledEventArgs> BackRequested;
        public void RaiseBackRequested(HandledEventArgs args)
        {
            if (BackRequested != null)
            {
                BackRequested.Invoke(this, args);
            }
            if (BackButtonHandling == BootStrapper.BackButton.Attach && !args.Handled && (args.Handled = Frame.BackStackDepth > 0))
            {
                GoBack();
            }
        }

        public event EventHandler<HandledEventArgs> ForwardRequested;
        public void RaiseForwardRequested(HandledEventArgs args)
        {
            ForwardRequested?.Invoke(this, args);

            if (!args.Handled && Frame.ForwardStack.Count > 0)
            {
                GoForward();
            }
        }

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

        public void ClearPageState(Type type)
        {
            this.FrameStateSettingsService().Remove(GetPageStateKey(FrameId, type, BackStackDepth));
        }

        #endregion

        #region frame facade

        public Frame Frame { get; }

        public BootStrapper.BackButton BackButtonHandling { get; internal set; }

        public string FrameId { get; set; } = string.Empty;

        internal NavigationService NavigationService { get; set; }

        public bool Navigate(Type page, object parameter, NavigationTransitionInfo infoOverride)
        {
            DebugWrite();

            if (Frame.Navigate(page, parameter, infoOverride))
            {
                return page.Equals(Frame.Content?.GetType());
            }
            else
            {
                return false;
            }
        }

        internal ISerializationService SerializationService => NavigationService.SerializationService;

        public void SetNavigationState(string state)
        {
            DebugWrite($"State {state}");

            Frame.SetNavigationState(state);
        }

        public string GetNavigationState()
        {
            DebugWrite();

            return Frame.GetNavigationState();
        }

        public int BackStackDepth => Frame.BackStackDepth;

        public bool CanGoBack => Frame.CanGoBack;

        public NavigationMode NavigationModeHint = NavigationMode.New;

        public void GoBack()
        {
            DebugWrite($"CanGoBack {CanGoBack}");

            NavigationModeHint = NavigationMode.Back;
            if (CanGoBack) Frame.GoBack();
        }

        public void Refresh()
        {
            DebugWrite();

            NavigationModeHint = NavigationMode.Refresh;

            try
            {
                Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
                // this only works for apps using serializable types
                var state = Frame.GetNavigationState();
                Frame.SetNavigationState(state);
            }
            catch (Exception)
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                    Frame.GoForward();
                }
                else if (Frame.CanGoForward)
                {
                    Frame.GoForward();
                    Frame.GoBack();
                }
                else
                {
                    // not much we can really do in this case
                    (Frame.Content as Page)?.UpdateLayout();
                }
            }
        }

        public bool CanGoForward => Frame.CanGoForward;

        public void GoForward()
        {
            DebugWrite($"CanGoForward {CanGoForward}");

            NavigationModeHint = NavigationMode.Forward;
            if (CanGoForward) Frame.GoForward();
        }

        public object Content => Frame.Content;

        public Type CurrentPageType { get; internal set; }

        public object CurrentPageParam { get; internal set; }

        public object GetValue(DependencyProperty dp) => Frame.GetValue(dp);

        public void SetValue(DependencyProperty dp, object value) { Frame.SetValue(dp, value); }

        public void ClearValue(DependencyProperty dp) { Frame.ClearValue(dp); }

        #endregion

        readonly List<EventHandler<NavigatedEventArgs>> _navigatedEventHandlers = new List<EventHandler<NavigatedEventArgs>>();
        public event EventHandler<NavigatedEventArgs> Navigated
        {
            add { if (!_navigatedEventHandlers.Contains(value)) _navigatedEventHandlers.Add(value); }
            remove { if (_navigatedEventHandlers.Contains(value)) _navigatedEventHandlers.Remove(value); }
        }
        void FacadeNavigatedEventHandler(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            DebugWrite();

            CurrentPageType = e.SourcePageType;
            CurrentPageParam = SerializationService.Deserialize(e.Parameter?.ToString());
            var args = new NavigatedEventArgs(e, Content as Page);
            if (NavigationModeHint != NavigationMode.New)
                args.NavigationMode = NavigationModeHint;
            NavigationModeHint = NavigationMode.New;
            foreach (var handler in _navigatedEventHandlers)
            {
                handler(this, args);
            }
        }

        readonly List<EventHandler<NavigatingEventArgs>> _navigatingEventHandlers = new List<EventHandler<NavigatingEventArgs>>();
        public event EventHandler<NavigatingEventArgs> Navigating
        {
            add { if (!_navigatingEventHandlers.Contains(value)) _navigatingEventHandlers.Add(value); }
            remove { if (_navigatingEventHandlers.Contains(value)) _navigatingEventHandlers.Remove(value); }
        }
        private async void FacadeNavigatingCancelEventHandler(object sender, NavigatingCancelEventArgs e)
        {
            DebugWrite();

            object parameter = null;
            try
            {
                parameter = SerializationService.Deserialize(e.Parameter?.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Your parameter must be serializable. If it isn't, then use SessionState.", ex);
            }
            var deferral = new DeferralManager();
            var args = new NavigatingEventArgs(deferral, e, Content as Page, parameter);
            if (NavigationModeHint != NavigationMode.New)
                args.NavigationMode = NavigationModeHint;
            NavigationModeHint = NavigationMode.New;
            _navigatingEventHandlers.ForEach(x => x(this, args));
            await deferral.WaitForDeferralsAsync();
            e.Cancel = args.Cancel;
        }
    }

}
