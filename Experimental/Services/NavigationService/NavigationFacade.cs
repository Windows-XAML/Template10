using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public class NavigationFacade
    {
        public NavigationFacade(Frame frame)
        {
            _frame = frame;
            _navigatedEventHandlers = new List<EventHandler<NavigationEventArgs>>();

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
            var page = this.CurrentPageType;
            var param = this.CurrentPageParam;
            this._frame.BackStack.Remove(this._frame.BackStack.Last());
            this.Navigate(page, param);
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

        readonly List<EventHandler<NavigationEventArgs>> _navigatedEventHandlers;
        public event EventHandler<NavigationEventArgs> Navigated
        {
            add
            {
                if (!_navigatedEventHandlers.Contains(value))
                {
                    _navigatedEventHandlers.Add(value);
                    if (_navigatedEventHandlers.Count == 1)
                        _frame.Navigated += FacadeNavigatedEventHandler;
                }
            }

            remove
            {
                if (_navigatedEventHandlers.Contains(value))
                {
                    _navigatedEventHandlers.Remove(value);
                    if (_navigatedEventHandlers.Count == 0)
                        _frame.Navigated -= FacadeNavigatedEventHandler;
                }
            }
        }

        void FacadeNavigatedEventHandler(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            foreach (var handler in _navigatedEventHandlers)
            {
                var args = new NavigationEventArgs()
                {
                    NavigationMode = e.NavigationMode,
                    Parameter = (e.Parameter == null) ? string.Empty : e.Parameter.ToString()
                };
                handler(this, args);
            }
            this.CurrentPageType = e.SourcePageType;
            this.CurrentPageParam = e.Parameter as String;
        }

        readonly List<EventHandler> _navigatingEventHandlers = new List<EventHandler>();
        public event EventHandler Navigating
        {
            add
            {
                if (!_navigatingEventHandlers.Contains(value))
                {
                    _navigatingEventHandlers.Add(value);
                    if (_navigatingEventHandlers.Count == 1)
                        _frame.Navigating += FacadeNavigatingCancelEventHandler;
                }
            }
            remove
            {
                if (_navigatingEventHandlers.Contains(value))
                {
                    _navigatingEventHandlers.Remove(value);
                    if (_navigatingEventHandlers.Count == 0)
                        _frame.Navigating -= FacadeNavigatingCancelEventHandler;
                }
            }
        }

        private void FacadeNavigatingCancelEventHandler(object sender, NavigatingCancelEventArgs e)
        {
            foreach (var handler in _navigatingEventHandlers)
            {
                handler(this, new EventArgs());
            }
        }
    }

}
