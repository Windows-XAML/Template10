using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace Template10.Behaviors
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-XamlBehaviors
    [Microsoft.Xaml.Interactivity.TypeConstraint(typeof(Button))]
    public class NavButtonBehavior : DependencyObject, IBehavior
    {
        Button element { get { return AssociatedObject as Button; } }
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
                Window.Current.SizeChanged += Current_SizeChanged;
            }
        }

        private long _goBackReg;
        private long _goForwardReg;
        public void Detach()
        {
            element.Click -= Element_Click;
            Window.Current.SizeChanged -= Current_SizeChanged;
            UnregisterPropertyChangedCallback(Frame.CanGoBackProperty, _goBackReg);
            UnregisterPropertyChangedCallback(Frame.CanGoForwardProperty, _goForwardReg);
        }

        private void Element_Click(object sender, RoutedEventArgs e)
        {
            switch (Direction)
            {
                case Directions.Back:
                    if (Frame.CanGoBack) Frame.GoBack();
                    break;
                case Directions.Forward:
                    if (Frame.CanGoForward) Frame.GoForward();
                    break;
            }
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e) { Calculate(); }

        private void Calculate()
        {
            System.Diagnostics.Debug.WriteLine("Calculate + " + Direction);
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
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(Directions),
                typeof(NavButtonBehavior), new PropertyMetadata(Directions.Back));

        public Frame Frame
        {
            get { return (Frame)GetValue(FrameProperty); }
            set
            {
                if (Frame == null)
                {
                    _goBackReg = value.RegisterPropertyChangedCallback(Frame.CanGoBackProperty, (s, e) => Calculate());
                    _goForwardReg = value.RegisterPropertyChangedCallback(Frame.CanGoForwardProperty, (s, e) => Calculate());
                }
                SetValue(FrameProperty, value);
            }
        }
        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register("Frame", typeof(Frame),
                typeof(NavButtonBehavior), new PropertyMetadata(null, (d, e) => { (d as NavButtonBehavior).Calculate(); }));

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

            // mobile always has a visible back button
            var mobilefam = ResourceContext.GetForCurrentView().QualifierValues["DeviceFamily"].Equals("Mobile");
            if (mobilefam)
                return Visibility.Collapsed;

            // simply don't know what to do with else
            var desktopfam = ResourceContext.GetForCurrentView().QualifierValues["DeviceFamily"].Equals("Desktop");
            if (!desktopfam)
                return Visibility.Collapsed;

            // touch always has a visible back button
            var touchmode = UIViewSettings.GetForCurrentView().UserInteractionMode == UserInteractionMode.Touch;
            if (touchmode)
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
