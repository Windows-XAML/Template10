using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using Template10.Utils;
using System;
using Template10.Common;

namespace Template10.Behaviors
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-XamlBehaviors
    [Microsoft.Xaml.Interactivity.TypeConstraint(typeof(Button))]
    public class NavButtonBehavior : DependencyObject, IBehavior
    {
        private long _goBackReg;
        private long _goForwardReg;
        private IDispatcherWrapper _dispatcher;
        private EventThrottleHelper _throttleHelper;

        Button element => AssociatedObject as Button;
        public DependencyObject AssociatedObject { get; set; }

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;

            // process start
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                element.Visibility = Visibility.Visible;
            }
            else
            {
                _dispatcher = Common.DispatcherWrapper.Current();

                // throttled calculate event
                _throttleHelper = new EventThrottleHelper();
                _throttleHelper.ThrottledEvent += delegate { Calculate(); };
                _throttleHelper.Throttle = 1000;

                // handle click
                element.Click += new Common.WeakReference<NavButtonBehavior, object, RoutedEventArgs>(this)
                {
                    EventAction = (i, s, e) => Element_Click(s, e),
                    DetachAction = (i, w) => element.Click -= w.Handler,
                }.Handler;
                CalculateThrottled();
            }
        }

        public void Detach()
        {
            _throttleHelper = null;
            if (Frame != null)
            {
                Frame.SizeChanged -= SizeChanged;
                Frame.LayoutUpdated -= LayoutUpdated;
            }
            UnregisterPropertyChangedCallback(Frame.CanGoBackProperty, _goBackReg);
            UnregisterPropertyChangedCallback(Frame.CanGoForwardProperty, _goForwardReg);
        }

        volatile bool _letLayoutUpdatedInvoke = false;
        private void SizeChanged(object sender, SizeChangedEventArgs e) { _letLayoutUpdatedInvoke = true; }
        private void LayoutUpdated(object sender, object e)
        {
            if (_letLayoutUpdatedInvoke)
            {
                _letLayoutUpdatedInvoke = false;
                CalculateThrottled();
            }
        }

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
            _dispatcher.Dispatch(() =>
            {
                switch (Direction)
                {
                    case Directions.Back:
                        {
                            element.Visibility = CalculateBackVisibility(Frame);
                            break;
                        }
                    case Directions.Forward:
                        {
                            element.Visibility = CalculateForwardVisibility(Frame);
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
            var behavior = (d as NavButtonBehavior);
            var frame = args.NewValue as Frame;
            if (frame != null)
            {
                behavior._goBackReg = frame.RegisterPropertyChangedCallback(Frame.CanGoBackProperty, (s, e) => behavior.CalculateThrottled());
                behavior._goForwardReg = frame.RegisterPropertyChangedCallback(Frame.CanGoForwardProperty, (s, e) => behavior.CalculateThrottled());
                frame.SizeChanged += behavior.SizeChanged;
                frame.LayoutUpdated += behavior.LayoutUpdated;
            }
            behavior.CalculateThrottled();
        }

        public static Visibility CalculateForwardVisibility(Frame frame)
        {
            // in some cases frame may be null, esp. race conditions
            if (frame == null)
                return Visibility.Collapsed;

            // by design it is not visible when not applicable
            var cangoforward = frame.CanGoForward;
            if (!cangoforward)
                return Visibility.Collapsed;

            // at this point, we show the on-canvas button
            return Visibility.Visible;
        }

        public static Visibility CalculateBackVisibility(Frame frame)
        {
            // in some cases frame may be null, esp. race conditions
            if (frame == null)
                return Visibility.Collapsed;

            // by design it is not visible when not applicable
            var cangoback = frame.CanGoBack;
            if (!cangoback)
                return Visibility.Collapsed;

            // continuum mode
            var touchmode = UIViewSettings.GetForCurrentView().UserInteractionMode == UserInteractionMode.Touch;

            // mobile always has a visible back button
            var mobilefam = "Windows.Mobile".Equals(Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily, StringComparison.OrdinalIgnoreCase);
            if (mobilefam && touchmode)
                // this means phone, not continuum, which uses hardware (or steering wheel) back button
                return Visibility.Collapsed;

            // handle iot
            var iotfam = "Windows.IoT".Equals(Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily, StringComparison.OrdinalIgnoreCase);
            if (!iotfam)
            {
                // simply don't know what to do with else
                var desktopfam = "Windows.Desktop".Equals(Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily, StringComparison.OrdinalIgnoreCase);
                if (!desktopfam)
                    return Visibility.Collapsed;
            }

            // touch always has a visible back button (continuum)
            if (!iotfam && touchmode)
                return Visibility.Collapsed;

            // full screen back button is only visible when mouse reveals title (prefered behavior is on-canvas visible)
            var fullscreen = ApplicationView.GetForCurrentView().IsFullScreenMode; // don't confuse with IsFullScreen
            if (fullscreen)
                return Visibility.Visible;

            // hide the button if the shell button is visible
            var optinback = SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility.Equals(AppViewBackButtonVisibility.Visible);
            if (optinback)
            {
                // shell button will not be visible if there is no title bar
                var hastitle = !CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar;
                if (hastitle)
                    return Visibility.Collapsed;
            }

            // at this point, we show the on-canvas button
            return Visibility.Visible;
        }
    }
}
