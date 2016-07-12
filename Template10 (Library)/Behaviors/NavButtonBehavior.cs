using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using Template10.Utils;
using System;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Navigation;
using Template10.Common;

namespace Template10.Behaviors
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-XamlBehaviors
    [Microsoft.Xaml.Interactivity.TypeConstraint(typeof(Button))]
    public class NavButtonBehavior : DependencyObject, IBehavior
    {
        private readonly IDispatcherWrapper _dispatcher;
        private readonly EventThrottleHelper _throttleHelper;
        private DeviceUtils _deviceUtils;

        Button element => AssociatedObject as Button;

        public DependencyObject AssociatedObject { get; set; }

        public NavButtonBehavior()
        {
            _dispatcher = Common.DispatcherWrapper.Current();
            _throttleHelper = new EventThrottleHelper { Throttle = 1000 };
        }

        private void ThrottleHelperOnThrottledEvent(object sender, object o)
        {
            Calculate();
        }

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            _throttleHelper.ThrottledEvent += ThrottleHelperOnThrottledEvent;

            // process start
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                element.Visibility = Visibility.Visible;
            }
            else
            {
                // handle click
                element.Click += new Common.WeakReference<NavButtonBehavior, object, RoutedEventArgs>(this)
                {
                    EventAction = (i, s, e) => i.Element_Click(s, e),
                    DetachAction = (i, w) => element.Click -= w.Handler,
                }.Handler;
                CalculateThrottled();
                if (BootStrapper.Current != null) BootStrapper.Current.ShellBackButtonUpdated += Current_ShellBackButtonUpdated;
                _deviceUtils = DeviceUtils.Current();
                if (_deviceUtils != null) _deviceUtils.Changed += DispositionChanged;
            }
        }

        private void DispositionChanged(object sender, EventArgs eventArgs)
        {
            CalculateThrottled();
        }

        public void Detach()
        {
            _throttleHelper.ThrottledEvent -= ThrottleHelperOnThrottledEvent;
            DetachFrameEvents(this, Frame);
            if (BootStrapper.Current != null) BootStrapper.Current.ShellBackButtonUpdated -= Current_ShellBackButtonUpdated;
            if (_deviceUtils != null) _deviceUtils.Changed -= DispositionChanged;
        }

        private void Current_ShellBackButtonUpdated(object sender, EventArgs e) => Calculate();

        private void OnNavigated(object sender, NavigationEventArgs navigationEventArgs) => CalculateThrottled();

        private void FrameOnLoaded(object sender, RoutedEventArgs routedEventArgs) => CalculateThrottled();

        private void CalculateThrottled()
        {
            _throttleHelper?.DispatchTriggerEvent(null);
        }

        private void Element_Click(object sender, RoutedEventArgs e)
        {
            var nav = Services.NavigationService.NavigationService.GetForFrame(Frame);
            if (nav == null)
            {
                switch (Direction)
                {
                    case Directions.Back:
                        {
                            if (Frame?.CanGoBack ?? false) Frame.GoBack();
                            break;
                        }
                    case Directions.Forward:
                        {
                            if (Frame?.CanGoForward ?? false) Frame.GoForward();
                            break;
                        }
                }
            }
            else
            {
                switch (Direction)
                {
                    case Directions.Back:
                        {
                            nav.GoBack();
                            break;
                        }
                    case Directions.Forward:
                        {
                            nav.GoForward();
                            break;
                        }
                }
            }
        }

        private void Calculate()
        {
            // just in case
            if (element == null)
                return;

            // make changes on UI thread
            _dispatcher?.Dispatch(() =>
            {
                switch (Direction)
                {
                    case Directions.Back:
                        {
                            element.Visibility = NavButtonsHelper.CalculateBackVisibility(Frame);
                            break;
                        }
                    case Directions.Forward:
                        {
                            element.Visibility = NavButtonsHelper.CalculateForwardVisibility(Frame);
                            break;
                        }
                }
            });
        }

        public enum Directions { Back, Forward }

        public Directions Direction { get { return (Directions)GetValue(DirectionProperty); } set { SetValue(DirectionProperty, value); } }

        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(nameof(Direction),
            typeof(Directions), typeof(NavButtonBehavior), new PropertyMetadata(Directions.Back));

        public Frame Frame { get { return (Frame)GetValue(FrameProperty); } set { SetValue(FrameProperty, value); } }

        public static readonly DependencyProperty FrameProperty = DependencyProperty.Register(nameof(Frame),
            typeof(Frame), typeof(NavButtonBehavior), new PropertyMetadata(null, FrameChanged));

        private static void FrameChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var behavior = (NavButtonBehavior)d;
            DetachFrameEvents(behavior, args.OldValue as Frame);
            AttachFrameEvents(behavior, args.NewValue as Frame);
            behavior.CalculateThrottled();
        }

        private readonly ConditionalWeakTable<Frame, FrameEventRegistration> _eventRegistrationInfo = new ConditionalWeakTable<Frame, FrameEventRegistration>();

        private static void AttachFrameEvents(NavButtonBehavior behavior, Frame frame)
        {
            if (behavior == null || frame == null)
            {
                return;
            }
            FrameEventRegistration eventReg;
            if (behavior._eventRegistrationInfo.TryGetValue(frame, out eventReg))
            {
                // events already attached
                return;
            }
            eventReg = new FrameEventRegistration();
            behavior._eventRegistrationInfo.Add(frame, eventReg);
            eventReg.GoBackReg = frame.RegisterPropertyChangedCallback(Frame.CanGoBackProperty, (s, e) => behavior.CalculateThrottled());
            eventReg.GoForwardReg = frame.RegisterPropertyChangedCallback(Frame.CanGoForwardProperty, (s, e) => behavior.CalculateThrottled());
            frame.Navigated += behavior.OnNavigated;
            frame.Loaded += behavior.FrameOnLoaded;
        }

        private static void DetachFrameEvents(NavButtonBehavior behavior, Frame frame)
        {
            if (behavior == null || frame == null)
            {
                return;
            }
            FrameEventRegistration eventReg;
            if (!behavior._eventRegistrationInfo.TryGetValue(frame, out eventReg))
            {
                // events already detached
                return;
            }
            behavior._eventRegistrationInfo.Remove(frame);
            frame.UnregisterPropertyChangedCallback(Frame.CanGoBackProperty, eventReg.GoBackReg);
            frame.UnregisterPropertyChangedCallback(Frame.CanGoForwardProperty, eventReg.GoForwardReg);
            frame.Navigated -= behavior.OnNavigated;
            frame.Loaded -= behavior.FrameOnLoaded;
        }

        private class FrameEventRegistration
        {
            public long GoBackReg;
            public long GoForwardReg;
        }
    }
}
