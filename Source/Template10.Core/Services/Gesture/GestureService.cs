using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Ioc;
using Prism.Logging;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;

namespace Template10.Services
{
    public class GestureService : IGestureService
    {
        private static readonly Dictionary<CoreWindow, IGestureService> _cache
            = new Dictionary<CoreWindow, IGestureService>();

        public static IGestureService GetForCurrentView(CoreWindow window = null)
        {
            if (!_cache.ContainsKey(window ?? Window.Current.CoreWindow))
            {
                throw new Exception("Not setup for current view.");
            }
            return _cache[Window.Current.CoreWindow];
        }

        public static void SetupWindowListeners(CoreWindow window)
        {
            if (_cache.ContainsKey(window))
            {
                throw new Exception("Already setup for current view.");
            }
            _cache.Add(window, new GestureService(window));

            window.Closed += Window_Closed;

            void Window_Closed(CoreWindow sender, CoreWindowEventArgs args)
            {
                window.Closed -= Window_Closed;
                if (_cache.ContainsKey(window))
                {
                    (_cache[window] as GestureService).Dispose(window);
                    _cache.Remove(window);
                }
            }
        }

        private GestureService(CoreWindow window)
        {
            window.Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;
            window.PointerPressed += CoreWindow_PointerPressed;
            SystemNavigationManager.GetForCurrentView().BackRequested += GestureService_BackRequested;
            _logger = ApplicationTemplate.Current.Container.Resolve<ILoggerFacade>();
        }

        public Dictionary<string, Action<KeyDownEventArgs>> KeyDownCallbacks { get; } = new Dictionary<string, Action<KeyDownEventArgs>>();
        public Dictionary<string, Action> BackRequestedCallbacks { get; } = new Dictionary<string, Action>();
        public Dictionary<string, Action> ForwardRequestedCallbacks { get; } = new Dictionary<string, Action>();
        public Dictionary<string, Action> MenuRequestedCallbacks { get; } = new Dictionary<string, Action>();
        public Dictionary<string, Action> RefreshRequestedCallbacks { get; } = new Dictionary<string, Action>();
        public Dictionary<string, Action> SearchRequestedCallbacks { get; } = new Dictionary<string, Action>();

        private readonly ILoggerFacade _logger;

        #region Blocker

        private readonly List<GestureBlocker> _blockers = new List<GestureBlocker>();
        public GestureBlocker CreateBlocker(Gesture gesture, BlockerPeriod period)
        {
            GestureBlocker blocker = null;
            blocker = new GestureBlocker(gesture, period)
            {
                Remove = () => _blockers.Remove(blocker)
            };
            _blockers.Add(blocker);
            return blocker;
        }

        private bool RaiseAnyBlockers(Gesture gesture)
        {
            var blockers = _blockers.Where(x => x.Gesture.Equals(gesture));
            foreach (var blocker in blockers)
            {
                blocker.RaiseEvent();
            }
            return blockers.Any();
        }

        #endregion

        public void RaiseRefreshRequested()
        {
            if (!RaiseAnyBlockers(Gesture.Refresh))
            {
                foreach (var item in RefreshRequestedCallbacks)
                {
                    item.Value();
                }
            }
        }
        public void RaiseBackRequested()
        {
            if (!RaiseAnyBlockers(Gesture.Back))
            {
                foreach (var item in BackRequestedCallbacks)
                {
                    item.Value();
                }
            }
        }
        public void RaiseForwardRequested()
        {
            if (!RaiseAnyBlockers(Gesture.Forward))
            {
                foreach (var item in ForwardRequestedCallbacks)
                {
                    item.Value();
                }
            }
        }
        public void RaiseSearchRequested()
        {
            if (!RaiseAnyBlockers(Gesture.Search))
            {
                foreach (var item in SearchRequestedCallbacks)
                {
                    item.Value();
                }
            }
        }
        public void RaiseMenuRequested()
        {
            if (!RaiseAnyBlockers(Gesture.Menu))
            {
                foreach (var item in MenuRequestedCallbacks)
                {
                    item.Value();
                }
            }
        }

        private void Dispose(CoreWindow window)
        {
            window.Dispatcher.AcceleratorKeyActivated -= Dispatcher_AcceleratorKeyActivated;
            window.PointerPressed -= CoreWindow_PointerPressed;
            SystemNavigationManager.GetForCurrentView().BackRequested -= GestureService_BackRequested;
        }

        private void GestureService_BackRequested(object sender, BackRequestedEventArgs e)
        {
            RaiseBackRequested();
        }

        private void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs e)
        {
            var properties = e.CurrentPoint.Properties;
            // Ignore button chords with the left, right, and middle buttons
            if (properties.IsLeftButtonPressed
                || properties.IsRightButtonPressed
                || properties.IsMiddleButtonPressed)
            {
                return;
            }
            TestForNavigateRequested(e, properties);
        }

        private void Dispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs e)
        {
            if (!e.EventType.ToString().Contains("Down") || e.Handled)
            {
                return;
            }
            var args = new KeyDownEventArgs(e.VirtualKey) { EventArgs = e };
            TestForSearchRequested(args);
            TestForMenuRequested(args);
            TestForNavigateRequested(args);
            foreach (var item in KeyDownCallbacks.ToArray())
            {
                item.Value?.Invoke(args);
            }
        }

        private void TestForNavigateRequested(KeyDownEventArgs e)
        {
            if ((e.VirtualKey == VirtualKey.GoBack)
                || (e.VirtualKey == VirtualKey.NavigationLeft)
                || (e.VirtualKey == VirtualKey.GamepadMenu)
                || (e.VirtualKey == VirtualKey.GamepadLeftShoulder)
                || (e.OnlyAlt && e.VirtualKey == VirtualKey.Back)
                || (e.OnlyAlt && e.VirtualKey == VirtualKey.Left))
            {
                _logger.Log($"{nameof(GestureService)}.BackRequested", Category.Info, Priority.None);
                RaiseBackRequested();
            }
            else if ((e.VirtualKey == VirtualKey.GoForward)
                || (e.VirtualKey == VirtualKey.NavigationRight)
                || (e.VirtualKey == VirtualKey.GamepadRightShoulder)
                || (e.OnlyAlt && e.VirtualKey == VirtualKey.Right))
            {
                _logger.Log($"{nameof(GestureService)}.ForwardRequested", Category.Info, Priority.None);
                RaiseForwardRequested();
            }
            else if ((e.VirtualKey == VirtualKey.Refresh)
                || (e.VirtualKey == VirtualKey.F5))
            {
                _logger.Log($"{nameof(GestureService)}.RefreshRequested", Category.Info, Priority.None);
                RaiseRefreshRequested();
            }
            // this is still a preliminary value?
            else if ((e.VirtualKey == VirtualKey.M) && e.OnlyAlt)
            {
                _logger.Log($"{nameof(GestureService)}.MenuRequested", Category.Info, Priority.None);
                RaiseMenuRequested();
            }
        }

        private void TestForNavigateRequested(PointerEventArgs e, PointerPointProperties properties)
        {
            // If back or foward are pressed (but not both) 
            var backPressed = properties.IsXButton1Pressed;
            var forwardPressed = properties.IsXButton2Pressed;
            if (backPressed ^ forwardPressed)
            {
                e.Handled = true;
                if (backPressed)
                {
                    _logger.Log($"{nameof(GestureService)}.BackRequested", Category.Info, Priority.None);
                    RaiseBackRequested();
                }
                else if (forwardPressed)
                {
                    _logger.Log($"{nameof(GestureService)}.ForwardRequested", Category.Info, Priority.None);
                    RaiseForwardRequested();
                }
            }
        }

        private void TestForMenuRequested(KeyDownEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.GamepadMenu)
            {
                _logger.Log($"{nameof(GestureService)}.MenuRequested", Category.Info, Priority.None);
                RaiseMenuRequested();
            }
        }

        private void TestForSearchRequested(KeyDownEventArgs args)
        {
            if (args.OnlyControl && args.Character.ToString().ToLower().Equals("e"))
            {
                _logger.Log($"{nameof(GestureService)}.SearchRequested", Category.Info, Priority.None);
                RaiseSearchRequested();
            }
        }
    }
}
