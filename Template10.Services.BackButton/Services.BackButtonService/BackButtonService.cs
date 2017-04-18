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
                if (e.VirtualKey == Windows.System.VirtualKey.GoBack) RaiseNavigateBack();
                else if (e.VirtualKey == Windows.System.VirtualKey.NavigationLeft) RaiseNavigateBack();
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadMenu) RaiseNavigateBack();
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadLeftShoulder) RaiseNavigateBack();
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Back) RaiseNavigateBack();
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Left) RaiseNavigateBack();

                // use this to nav forward
                else if (e.VirtualKey == Windows.System.VirtualKey.GoForward) RaiseNavigateForward();
                else if (e.VirtualKey == Windows.System.VirtualKey.NavigationRight) RaiseNavigateForward();
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadRightShoulder) RaiseNavigateForward();
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Right) RaiseNavigateForward();
            };
        }

        Windows.UI.Core.CoreDispatcher Dispatcher { get; set; }

        public static void RaiseNavigateBack() => NavigateBack?.Invoke(null, EventArgs.Empty);
        public static event EventHandler NavigateBack;

        public static void RaiseNavigateForward() => NavigateForward?.Invoke(null, EventArgs.Empty);
        public static event EventHandler NavigateForward;
    }
}
