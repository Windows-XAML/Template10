using System;
using System.Runtime.CompilerServices;

namespace Template10.Services.KeyboardService
{
    public class KeyboardService : KeyboardServiceBase, IKeyboardService
    {
        public static KeyboardService Instance { get; private set; } = new KeyboardService();

        private KeyboardService()
        {
            Helper.KeyDown = (e) =>
            {
                e.Handled = true;

                // use this to place focus in search box
                if (e.OnlyControl && e.Character.ToString().ToLower().Equals("e")) AfterControlEGesture?.Invoke();

                // use this for hamburger menu
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadMenu) AfterMenuGesture?.Invoke();

                // anything else
                else e.Handled = false;
            };
        }

        public Action AfterControlEGesture { get; set; }

        public Action AfterMenuGesture { get; set; }
    }

}
