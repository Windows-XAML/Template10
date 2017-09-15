using System;
using Template10.Common;
using Windows.Foundation;

namespace Template10.Services.Gesture
{
    public class KeyboardService : IKeyboardService, IKeyboardService2
    {
        public KeyboardService()
        {
            Setup();
        }

        KeyboardHelper _helper;

        /// <remarks>
        /// This must be called AFTER the first window is created.
        /// </remarks>
        private void Setup()
        {
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
