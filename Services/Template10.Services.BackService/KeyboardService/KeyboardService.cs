using System;
using Template10.Common;
using Windows.Foundation;

namespace Template10.Services.Gesture
{
    public class KeyboardService : IKeyboardService
    {
        public KeyboardService()
        {
            // empty
        }

        KeyboardHelper _helper;

        bool _Setup = false;
        /// <remarks>
        /// This must be called AFTER the first window is created.
        /// </remarks>
        public void Setup()
        {
            if (_Setup) throw new Exception("KeyboardService.Setup can only be called one time");
            else _Setup = true;

            _helper = new KeyboardHelper();
            _helper.KeyDown = (e) =>
                {
                    e.Handled = true;

                    if (e.OnlyControl && e.Character.ToString().ToLower().Equals("e"))
                    {
                        AfterSearchGesture?.Invoke(this, EventArgs.Empty);
                    }
                    else if (e.VirtualKey == Windows.System.VirtualKey.GamepadMenu)
                    {
                        AfterMenuGesture?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        e.Handled = false;
                    }

                    AfterKeyDown?.Invoke(this, e);
                };
        }

        public event EventHandler AfterSearchGesture;
        public event EventHandler AfterMenuGesture;
        public event TypedEventHandler<KeyboardEventArgs> AfterKeyDown;
    }

}
