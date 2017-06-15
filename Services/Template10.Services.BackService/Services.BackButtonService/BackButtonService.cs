using System;
using System.ComponentModel;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Template10.Services.BackButtonService
{
    public class BackButtonService
    {
        static BackButtonService()
        {
            var keyHelper = new KeyboardService.KeyboardHelper();
            keyHelper.KeyDown = (e) =>
            {
                e.Handled = true;

                // use this to nav back
                if (e.VirtualKey == Windows.System.VirtualKey.GoBack) e.Handled = RaiseBackRequested().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.NavigationLeft) e.Handled = RaiseBackRequested().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadMenu) e.Handled = RaiseBackRequested().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadLeftShoulder) e.Handled = RaiseBackRequested().Handled;
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Back) e.Handled = RaiseBackRequested().Handled;
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Left) e.Handled = RaiseBackRequested().Handled;

                // use this to nav forward
                else if (e.VirtualKey == Windows.System.VirtualKey.GoForward) e.Handled = RaiseForwardRequested().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.NavigationRight) e.Handled = RaiseForwardRequested().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadRightShoulder) e.Handled = RaiseForwardRequested().Handled;
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Right) e.Handled = RaiseForwardRequested().Handled;
            };

            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) =>
            {
                e.Handled = RaiseBackRequested().Handled;
            };
        }

        public static CancelEventArgs RaiseBeforeBackRequested()
        {
            var args = new CancelEventArgs();
            BeforeBackRequested?.Invoke(null, args);
            return args;
        }

        /// <summary>
        /// This event allows a mechainsm to intercept BackRequested and stop it. Some
        /// use cases would include the ModalDialog which would cancel the event, using
        /// it instead for itself to close the dialog - not wanting it to navigate a frame.
        /// </summary>
        public static event Common.TypedEventHandler<CancelEventArgs> BeforeBackRequested;

        public static Common.HandledEventArgs RaiseBackRequested()
        {
            if (RaiseBeforeBackRequested().Cancel)
            {
                return new Common.HandledEventArgs
                {
                    Handled = true
                };
            }

            var args = new Common.HandledEventArgs();
            BackRequested?.Invoke(null, args);
            return args;
        }
        public static event Common.TypedEventHandler<Common.HandledEventArgs> BackRequested;

        public static Common.HandledEventArgs RaiseForwardRequested()
        {
            var args = new Common.HandledEventArgs();
            ForwardRequested?.Invoke(null, args);
            return args;
        }
        public static event Common.TypedEventHandler<Common.HandledEventArgs> ForwardRequested;

        public static void UpdateShellBackButton(bool canGoBack)
        {
            // show the shell back only if there is anywhere to go in the default frame
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                (Settings.ShowShellBackButton && (canGoBack || Settings.ForceShowShellBackButton))
                    ? AppViewBackButtonVisibility.Visible
                    : AppViewBackButtonVisibility.Collapsed;
            ShellBackButtonUpdated?.Invoke(null, EventArgs.Empty);
        }

        public static event EventHandler ShellBackButtonUpdated;
    }

    public static class Settings
    {
        public static bool ShowShellBackButton { get; set; } = true;
        public static bool ForceShowShellBackButton { get; set; } = false;
    }
}
