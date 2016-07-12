using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Common
{
    /// <summary>
    /// Helper class for navigation buttons visibility logic.
    /// </summary>
    /// <remarks>Moved logic here from NavButtonBehavior because public static methods should be in a static class.</remarks>
    public static class NavButtonsHelper
    {
        /// <summary>
        /// Calculate forward navigation button visibility for a frame.
        /// </summary>
        /// <param name="frame">Frame.</param>
        /// <returns>Button visibility.</returns>
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

        /// <summary>
        /// Calculate forward navigation button visibility for a frame.
        /// </summary>
        /// <param name="frame">Frame.</param>
        /// <returns>Button visibility.</returns>
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

            // store value locally to avoid multiple WinRT interop calls
            var deviceFamily = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;

            // mobile always has a visible back button
            var mobilefam = "Windows.Mobile".Equals(deviceFamily, StringComparison.OrdinalIgnoreCase);
            if (mobilefam && touchmode)
                // this means phone, not continuum, which uses hardware (or steering wheel) back button
                return Visibility.Collapsed;

            // handle iot
            var iotfam = "Windows.IoT".Equals(deviceFamily, StringComparison.OrdinalIgnoreCase);
            if (!iotfam)
            {
                // simply don't know what to do with else
                var desktopfam = "Windows.Desktop".Equals(deviceFamily, StringComparison.OrdinalIgnoreCase);
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