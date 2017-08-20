using System;

namespace Template10.Services.KeyboardService
{
    public class KeyboardService : IKeyboardService
    {
        public static KeyboardService Instance { get; private set; } = new KeyboardService();

        KeyboardHelper _helper;

        bool _Setup = false;
        /// <remarks>
        /// This must be called AFTER the first window is created.
        /// </remarks>
        public void Setup()
        {
            if (_Setup) return;
            else _Setup = true;
            _helper = new KeyboardHelper()
            {
                KeyDown = (e) =>
                    {
                        e.Handled = true;

                        // use this to place focus in search box
                        if (e.OnlyControl && e.Character.ToString().ToLower().Equals("e")) AfterControlEGesture?.Invoke();

                        // use this for hamburger menu
                        else if (e.VirtualKey == Windows.System.VirtualKey.GamepadMenu) AfterMenuGesture?.Invoke();

                        // anything else
                        else e.Handled = false;

                        AfterKeyDown?.Invoke(e);
                    }
            };
        }

        public Action AfterControlEGesture { get; set; }

        public Action AfterMenuGesture { get; set; }

        public Action<KeyboardEventArgs> AfterKeyDown { get; set; }
    }

}
