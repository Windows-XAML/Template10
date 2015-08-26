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
            if (args.EventType == CoreAcceleratorKeyEventType.SystemKeyDown
                || args.EventType == CoreAcceleratorKeyEventType.KeyDown)
            {
                HandleKeyDown(args);
            }
        }

        private void HandleKeyDown(AcceleratorKeyEventArgs args)
        {
            var alt = (Window.Current.CoreWindow.GetKeyState(VirtualKey.Menu) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
            var shift = (Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
            var control = (Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
            var windows = ((Window.Current.CoreWindow.GetKeyState(VirtualKey.LeftWindows) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down) || ((Window.Current.CoreWindow.GetKeyState(VirtualKey.RightWindows) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down);
            var character = ToChar(args.VirtualKey, shift);

            System.Diagnostics.Debug.WriteLine("{0} alt:{1} shift:{2} control:{3} windows:{4} virt:{5}", character, alt, shift, control, windows, args.VirtualKey);

            var keyDown = new KeyboardEventArgs
            {
                AltKey = alt,
                Character = character,
                ControlKey = control,
                EventArgs = args,
                ShiftKey = shift,
                VirtualKey = args.VirtualKey
            };
            try { KeyDown?.Invoke(keyDown); } catch { }

            if (windows && (character == 'z' || character == 'Z'))
            {
                RaiseWindowZGestured();
            }
            else if (args.KeyStatus.IsExtendedKey && args.VirtualKey == VirtualKey.Back)
            {
                RaiseGoBackGestured();
            }
            else
            {
                return;
            }
            args.Handled = true;
        }

        private void zCoreDispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs e)
        {
            if ((e.EventType == CoreAcceleratorKeyEventType.SystemKeyDown ||
                e.EventType == CoreAcceleratorKeyEventType.KeyDown))
            {
                var coreWindow = Windows.UI.Xaml.Window.Current.CoreWindow;
                var downState = CoreVirtualKeyStates.Down;
                var virtualKey = e.VirtualKey;
                bool winKey = ((coreWindow.GetKeyState(VirtualKey.LeftWindows) & downState) == downState || (coreWindow.GetKeyState(VirtualKey.RightWindows) & downState) == downState);
                bool altKey = (coreWindow.GetKeyState(VirtualKey.Menu) & downState) == downState;
                bool controlKey = (coreWindow.GetKeyState(VirtualKey.Control) & downState) == downState;
                bool shiftKey = (coreWindow.GetKeyState(VirtualKey.Shift) & downState) == downState;
                bool noModifiers = !altKey && !controlKey && !shiftKey;
                bool onlyAlt = altKey && !controlKey && !shiftKey;

                // raise keydown actions
                var keyDown = new KeyboardEventArgs
                {
                    AltKey = altKey,
                    Character = ToChar(virtualKey, shiftKey),
                    ControlKey = controlKey,
                    EventArgs = e,
                    ShiftKey = shiftKey,
                    VirtualKey = virtualKey
                };

                try { KeyDown?.Invoke(keyDown); }
                catch { }

                if (((int)virtualKey == 166 && noModifiers) || (virtualKey == VirtualKey.Left && onlyAlt))
                {
                    // When the previous key or Alt+Left are pressed navigate back
                    e.Handled = true;
                    RaiseGoBackGestured();
                }
                else if (((int)virtualKey == 167 && noModifiers) || (virtualKey == VirtualKey.Right && onlyAlt))
                {
                    // When the next key or Alt+Right are pressed navigate forward
                    e.Handled = true;
                    RaiseGoForwardGestured();
                }
                else if (((int)virtualKey == 69 && controlKey))
                {
                    // when control-E
                    e.Handled = true;
                    RaiseControlEGestured();
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
                if (backPressed) RaiseGoBackGestured();
                if (forwardPressed) RaiseGoForwardGestured();
            }
        }

        public Action GoForwardGestured { get; set; }
        protected void RaiseGoForwardGestured()
        {
            try { GoForwardGestured?.Invoke(); }
            catch { }
        }

        public Action GoBackGestured { get; set; }
        protected void RaiseGoBackGestured()
        {
            try { GoBackGestured?.Invoke(); }
            catch { }
        }

        public Action ControlEGestured { get; set; }
        protected void RaiseControlEGestured()
        {
            try { ControlEGestured?.Invoke(); }
            catch { }
        }

        public Action WindowZGestured { get; set; }
        protected void RaiseWindowZGestured()
        {
            try { WindowZGestured?.Invoke(); }
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
