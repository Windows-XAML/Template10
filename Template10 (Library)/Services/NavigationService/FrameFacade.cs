﻿using System;
using System.Collections.Generic;
using Template10.Common;
using Template10.Services.SettingsService;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-NavigationService
    public class FrameFacade
    {
        internal FrameFacade(Frame frame)
        {
            Frame = frame;
            frame.Navigated += (s, e) => FacadeNavigatedEventHandler(s, e);
            frame.Navigating += (s, e) => FacadeNavigatingCancelEventHandler(s, e);

            // setup animations
            var c = new TransitionCollection { };
            var t = new NavigationThemeTransition { };
            var i = new EntranceNavigationTransitionInfo();
            t.DefaultNavigationTransitionInfo = i;
            c.Add(t);
            Frame.ContentTransitions = c;
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

        private string GetPageStateKey(Type type) => string.Format("{0}", type);

        public ISettingsService PageStateSettingsService(Type type)
        {
            return FrameStateSettingsService().Open(GetPageStateKey(type), true);
        }

        public void ClearPageState(Type type)
        {
            this.FrameStateSettingsService().Remove(GetPageStateKey(type));
        }

        #endregion

        #region frame facade

        public Frame Frame { get; }

        public BootStrapper.BackButton BackButtonHandling { get; internal set; }

        public string FrameId { get; set; } = string.Empty;

        public bool Navigate(Type page, object parameter, NavigationTransitionInfo infoOverride)
        {
            if (Frame.Navigate(page, parameter, infoOverride))
            {
                return page.Equals(Frame.Content?.GetType());
            }
            else
            {
                return false;
            }
        }

        public void SetNavigationState(string state) { Frame.SetNavigationState(state); }

        public string GetNavigationState() => Frame.GetNavigationState();

        public int BackStackDepth => Frame.BackStackDepth;

        public bool CanGoBack => Frame.CanGoBack;

        public NavigationMode NavigationModeHint = NavigationMode.New;

        public void GoBack()
        {
            NavigationModeHint = NavigationMode.Back;
            if (CanGoBack) Frame.GoBack();
        }

        public void Refresh()
        {
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
            CurrentPageType = e.SourcePageType;
            CurrentPageParam = e.Parameter;
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
        private void FacadeNavigatingCancelEventHandler(object sender, NavigatingCancelEventArgs e)
        {
            var args = new NavigatingEventArgs(e, Content as Page);
            if (NavigationModeHint != NavigationMode.New)
                args.NavigationMode = NavigationModeHint;
            NavigationModeHint = NavigationMode.New;
            foreach (var handler in _navigatingEventHandlers)
            {
                handler(this, args);
            }
            e.Cancel = args.Cancel;
        }
    }

}
