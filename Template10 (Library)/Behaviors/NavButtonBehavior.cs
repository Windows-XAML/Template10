using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using Template10.Utils;
using System;

namespace Template10.Behaviors
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-XamlBehaviors
    [Microsoft.Xaml.Interactivity.TypeConstraint(typeof(Button))]
    public class NavButtonBehavior : DependencyObject, IBehavior
    {
        bool update = false;
        private long _goBackReg;
        private long _goForwardReg;
        Button element => AssociatedObject as Button;
        public DependencyObject AssociatedObject { get; set; }

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                element.Visibility = Visibility.Visible;
            }
            else
            {
                element.Click += Element_Click;
                Calculate(true);
            }
        }

        public void Detach()
        {
            element.Click -= Element_Click;
            if (Frame != null)
            {

                Frame.SizeChanged -= SizeChanged;
                Frame.LayoutUpdated -= LayoutUpdated;
            }
            UnregisterPropertyChangedCallback(Frame.CanGoBackProperty, _goBackReg);
            UnregisterPropertyChangedCallback(Frame.CanGoForwardProperty, _goForwardReg);
        }

        private void SizeChanged(object sender, SizeChangedEventArgs e) { update = true; }
        private void LayoutUpdated(object sender, object e) { Calculate(true); }

        private void Element_Click(object sender, RoutedEventArgs e)
        {
            var nav = Services.NavigationService.NavigationService.GetForFrame(Frame);
            if (nav == null)
            {
                switch (Direction)
                {
                    case Directions.Back:
                        if (Frame?.CanGoBack ?? false) Frame.GoBack();
                        break;
                    case Directions.Forward:
                        if (Frame?.CanGoForward ?? false) Frame.GoForward();
                        break;
                }
            }
            else
            {
                switch (Direction)
                {
                    case Directions.Back:
                        nav.GoBack();
                        break;
                    case Directions.Forward:
                        nav.GoForward();
                        break;
                }
            }
        }

        private void Calculate(bool allow = false)
        {
            if (!allow)
                if (!update)
                    return;
            update = false;
            if (element == null)
                return;
            switch (Direction)
            {
                case Directions.Back:
                    element.Visibility = CalculateBackVisibility(Frame);
                    break;
                case Directions.Forward:
                    element.Visibility = CalculateForwardVisibility(Frame);
                    break;
            }
        }

        public enum Directions { Back, Forward }
        public Directions Direction
        {
            get { return (Directions)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(nameof(Direction),
            typeof(Directions), typeof(NavButtonBehavior), new PropertyMetadata(Directions.Back));

        public Frame Frame
        {
            get { return (Frame)GetValue(FrameProperty); }
            set { SetValue(FrameProperty, value); }
        }
        public static readonly DependencyProperty FrameProperty = DependencyProperty.Register(nameof(Frame),
            typeof(Frame), typeof(NavButtonBehavior), new PropertyMetadata(null, FrameChanged));
        private static void FrameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var b = (d as NavButtonBehavior);
            var f = e.NewValue as Frame;
            if (f == null)
            {
                b._goBackReg = f.RegisterPropertyChangedCallback(Frame.CanGoBackProperty, (s, args) => b.Calculate(true));
                b._goForwardReg = f.RegisterPropertyChangedCallback(Frame.CanGoForwardProperty, (s, args) => b.Calculate(true));
                f.SizeChanged += b.SizeChanged;
                f.LayoutUpdated += b.LayoutUpdated;
            }
            b.Calculate(true);
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
            var mobilefam = ResourceContext.GetForCurrentView().QualifierValues["DeviceFamily"].Equals("Mobile");
            if (mobilefam && touchmode)
                // this means phone, not continuum, which uses hardware (or steering wheel) back button
                return Visibility.Collapsed;

            // handle iot
            var iotfam = ResourceContext.GetForCurrentView().QualifierValues["DeviceFamily"].Equals("IoT");
            if (!iotfam)
            {
                // simply don't know what to do with else
                var desktopfam = ResourceContext.GetForCurrentView().QualifierValues["DeviceFamily"].Equals("Desktop");
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
