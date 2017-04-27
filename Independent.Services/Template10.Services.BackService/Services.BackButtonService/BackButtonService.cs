using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                if (e.VirtualKey == Windows.System.VirtualKey.GoBack) e.Handled = RaiseNavigateBack().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.NavigationLeft) e.Handled = RaiseNavigateBack().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadMenu) e.Handled = RaiseNavigateBack().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadLeftShoulder) e.Handled = RaiseNavigateBack().Handled;
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Back) e.Handled = RaiseNavigateBack().Handled;
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Left) e.Handled = RaiseNavigateBack().Handled;

                // use this to nav forward
                else if (e.VirtualKey == Windows.System.VirtualKey.GoForward) e.Handled = RaiseNavigateForward().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.NavigationRight) e.Handled = RaiseNavigateForward().Handled;
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadRightShoulder) e.Handled = RaiseNavigateForward().Handled;
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Right) e.Handled = RaiseNavigateForward().Handled;
            };
        }

        public static Common.HandledEventArgs RaiseNavigateBack()
        {
            var args = new Common.HandledEventArgs();
            NavigateBack?.Invoke(null, args);
            return args;
        }

        public static event Common.TypedEventHandler<Common.HandledEventArgs> NavigateBack;

        public static Common.HandledEventArgs RaiseNavigateForward()
        {
            var args = new Common.HandledEventArgs();
            NavigateForward?.Invoke(null, args);
            return args;
        }

        public static event Common.TypedEventHandler<Common.HandledEventArgs> NavigateForward;
    }
}
