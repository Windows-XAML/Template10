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

        public static Common.HandledEventArgs RaiseBackRequested()
        {
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
   }
}
