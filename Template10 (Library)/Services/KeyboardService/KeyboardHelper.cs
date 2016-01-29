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
        CoreWindow win = Window.Current.CoreWindow;
        public KeyboardHelper()
        {
           win.Dispatcher.AcceleratorKeyActivated += CoreDispatcher_AcceleratorKeyActivated;
           win.PointerPressed += CoreWindow_PointerPressed;
        }

        public void Cleanup()
        {
            win.Dispatcher.AcceleratorKeyActivated -= CoreDispatcher_AcceleratorKeyActivated;
            win.PointerPressed -= CoreWindow_PointerPressed;
        }

        private void CoreDispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs e)
        {
            if (e.EventType.ToString().Contains("Down") && !e.Handled)
            {
                var args = KeyboardEventArgs(e.VirtualKey);
                args.EventArgs = e;

                try { KeyDown?.Invoke(args); }
                finally
                {
                    e.Handled = e.Handled;
                }
            }
        }

        public Action<KeyboardEventArgs> KeyDown { get; set; }

        private KeyboardEventArgs KeyboardEventArgs(VirtualKey key)
        {
            var alt = (win.GetKeyState(VirtualKey.Menu) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
            var shift = (win.GetKeyState(VirtualKey.Shift) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
            var control = (win.GetKeyState(VirtualKey.Control) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
            var windows = ((win.GetKeyState(VirtualKey.LeftWindows) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down)
                || ((win.GetKeyState(VirtualKey.RightWindows) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down);
            return new KeyboardEventArgs
            {
                AltKey = alt,
                ControlKey = control,
                ShiftKey = shift,
                WindowsKey = windows,
                VirtualKey = key,
                Character = ToChar(key, shift),
            };
        }

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

        public Action PointerGoBackGestured { get; set; }
        protected void RaisePointerGoBackGestured()
        {
            try { PointerGoBackGestured?.Invoke(); }
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
