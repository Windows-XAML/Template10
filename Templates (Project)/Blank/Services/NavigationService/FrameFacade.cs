using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public class FrameFacade
    {
        public FrameFacade(Frame frame)
        {
            _frame = frame;
            _navigatedEventHandlers = new List<EventHandler<NavigatedEventArgs>>();

            // setup animations
            var c = new TransitionCollection { };
            var t = new NavigationThemeTransition { };
            //var i = new EntranceNavigationTransitionInfo();
            //t.DefaultNavigationTransitionInfo = i;
            c.Add(t);
            _frame.ContentTransitions = c;
        }

        #region frame facade

        readonly Frame _frame;

        public bool Navigate(Type page, string parameter) { return _frame.Navigate(page, parameter); }

        public void SetNavigationState(string state) { _frame.SetNavigationState(state); }

        public string GetNavigationState() { return _frame.GetNavigationState(); }

        public int BackStackDepth { get { return _frame.BackStackDepth; } }

        public bool CanGoBack { get { return _frame.CanGoBack; } }

        public void GoBack() { _frame.GoBack(); }

        public void Refresh()
        {
            var page = CurrentPageType;
            var param = CurrentPageParam;
            _frame.BackStack.Remove(_frame.BackStack.Last());
            Navigate(page, param);
        }

        public bool CanGoForward { get { return _frame.CanGoForward; } }

        public void GoForward() { _frame.GoForward(); }

        public object Content { get { return _frame.Content; } }

        public Type CurrentPageType { get; internal set; }

        public string CurrentPageParam { get; internal set; }

        public object GetValue(DependencyProperty dp) { return _frame.GetValue(dp); }

        public void SetValue(DependencyProperty dp, object value) { _frame.SetValue(dp, value); }

        public void ClearValue(DependencyProperty dp) { _frame.ClearValue(dp); }

        #endregion

        readonly List<EventHandler<NavigatedEventArgs>> _navigatedEventHandlers;
        public event EventHandler<NavigatedEventArgs> Navigated
        {
            add
            {
                if (_navigatedEventHandlers.Contains(value))
                    return;
                _navigatedEventHandlers.Add(value);
                if (_navigatedEventHandlers.Count == 1)
                    _frame.Navigated += FacadeNavigatedEventHandler;
            }

            remove
            {
                if (!_navigatedEventHandlers.Contains(value))
                    return;
                _navigatedEventHandlers.Remove(value);
                if (_navigatedEventHandlers.Count == 0)
                    _frame.Navigated -= FacadeNavigatedEventHandler;
            }
        }

        void FacadeNavigatedEventHandler(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            var args = new NavigatedEventArgs(e);
            foreach (var handler in _navigatedEventHandlers)
            {
                handler(this, args);
            }
            CurrentPageType = e.SourcePageType;
            CurrentPageParam = e.Parameter as String;
        }

        readonly List<EventHandler<NavigatingEventArgs>> _navigatingEventHandlers = new List<EventHandler<NavigatingEventArgs>>();
        public event EventHandler<NavigatingEventArgs> Navigating
        {
            add
            {
                if (_navigatingEventHandlers.Contains(value))
                    return;
                _navigatingEventHandlers.Add(value);
                if (_navigatingEventHandlers.Count == 1)
                    _frame.Navigating += FacadeNavigatingCancelEventHandler;
            }
            remove
            {
                if (!_navigatingEventHandlers.Contains(value))
                    return;
                _navigatingEventHandlers.Remove(value);
                if (_navigatingEventHandlers.Count == 0)
                    _frame.Navigating -= FacadeNavigatingCancelEventHandler;
            }
        }

        private void FacadeNavigatingCancelEventHandler(object sender, NavigatingCancelEventArgs e)
        {
            var args = new NavigatingEventArgs(e);
            foreach (var handler in _navigatingEventHandlers)
            {
                handler(this, args);
            }
            e.Cancel = args.Cancel;
        }
    }

}
