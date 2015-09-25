using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Template10.Services.KeyboardService
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-KeyboardService
    public class KeyboardHelper
    {
        public KeyboardHelper()
        {
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += CoreDispatcher_AcceleratorKeyActivated;
            Window.Current.CoreWindow.PointerPressed += CoreWindow_PointerPressed;
        }

        public void Cleanup()
        {
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated -= CoreDispatcher_AcceleratorKeyActivated;
            Window.Current.CoreWindow.PointerPressed -= CoreWindow_PointerPressed;
        }

        private void CoreDispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            if (args.EventType.ToString().Contains("Down") && !args.Handled)
            {
                var alt = (Window.Current.CoreWindow.GetKeyState(VirtualKey.Menu) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
                var shift = (Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
                var control = (Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
                var windows = ((Window.Current.CoreWindow.GetKeyState(VirtualKey.LeftWindows) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down) || ((Window.Current.CoreWindow.GetKeyState(VirtualKey.RightWindows) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down);
                var character = ToChar(args.VirtualKey, shift);

                var keyDown = new KeyboardEventArgs
                {
                    AltKey = alt,
                    Character = character,
                    ControlKey = control,
                    EventArgs = args,
                    ShiftKey = shift,
                    VirtualKey = args.VirtualKey
                };

                try { KeyDown?.Invoke(keyDown); }
                finally
                {
                    args.Handled = keyDown.Handled;
                }
            }
        }

        public Action<KeyboardEventArgs> KeyDown { get; set; }

        /// <summary>
        /// Invoked on every mouse click, touch screen tap, or equivalent interaction when this
        /// page is active and occupies the entire window.  Used to detect browser-style next and
        /// previous mouse button clicks to navigate between pages.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="e">Event data describing the conditions that led to the event.</param>
        private void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs e)
        {
            var properties = e.CurrentPoint.Properties;

            // Ignore button chords with the left, right, and middle buttons
            if (properties.IsLeftButtonPressed || properties.IsRightButtonPressed ||
                properties.IsMiddleButtonPressed)
                return;

            // If back or foward are pressed (but not both) navigate appropriately
            bool backPressed = properties.IsXButton1Pressed;
            bool forwardPressed = properties.IsXButton2Pressed;
            if (backPressed ^ forwardPressed)
            {
                e.Handled = true;
                if (backPressed) RaisePointerGoBackGestured();
                if (forwardPressed) RaisePointerGoForwardGestured();
            }
        }

        public Action PointerGoForwardGestured { get; set; }
        protected void RaisePointerGoForwardGestured()
        {
            try { PointerGoForwardGestured?.Invoke(); }
            catch { }
        }

        public Action GoPointerBackGestured { get; set; }
        protected void RaisePointerGoBackGestured()
        {
            try { GoPointerBackGestured?.Invoke(); }
            catch { }
        }

        private static char? ToChar(VirtualKey key, bool shift)
        {
            // convert virtual key to char
            if (32 == (int)key)
                return ' ';

            VirtualKey search;

            // look for simple letter
            foreach (var letter in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            {
                if (Enum.TryParse<VirtualKey>(letter.ToString(), out search) && search.Equals(key))
                    return (shift) ? letter : letter.ToString().ToLower()[0];
            }

            // look for simple number
            foreach (var number in "1234567890")
            {
                if (Enum.TryParse<VirtualKey>("Number" + number.ToString(), out search) && search.Equals(key))
                    return number;
            }

            // not found
            return null;
        }
    }

    enum VKeyClass_EnUs
    {
        Control, // 0-31, 33-47, 91-95, 144-165
        Character, // 32, 48-90
        NumPad, // 96-111
        Function // 112 - 135
    }

    public enum VKeyCharacterClass
    {
        Space,
        Numeric,
        Alphabetic
    }

}
