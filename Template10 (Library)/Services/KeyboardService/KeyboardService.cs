﻿using System;

namespace Template10.Services.KeyboardService
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-KeyboardService
    public class KeyboardService
    {
        KeyboardHelper _helper;

        public KeyboardService()
        {
            _helper = new KeyboardHelper();
            _helper.KeyDown = (e) =>
            {
                e.Handled = true;

                // use this to hide and show the menu
                if (e.OnlyWindows && e.Character.ToString().ToLower().Equals("z"))
                    AfterWindowZGesture?.Invoke();

                // use this to place focus in search box
                else if (e.OnlyControl && e.Character.ToString().ToLower().Equals("e"))
                    AfterControlEGesture?.Invoke();

                // use this to nav back
                else if (e.VirtualKey == Windows.System.VirtualKey.GoBack)
                    AfterBackGesture?.Invoke();
                else if (e.VirtualKey == Windows.System.VirtualKey.NavigationLeft)
                    AfterBackGesture?.Invoke();
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadLeftShoulder)
                    AfterBackGesture?.Invoke();
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Back)
                    AfterBackGesture?.Invoke();
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Left)
                    AfterBackGesture?.Invoke();

                // use this to nav forward
                else if (e.VirtualKey == Windows.System.VirtualKey.GoForward)
                    AfterForwardGesture?.Invoke();
                else if (e.VirtualKey == Windows.System.VirtualKey.NavigationRight)
                    AfterForwardGesture?.Invoke();
                else if (e.VirtualKey == Windows.System.VirtualKey.GamepadRightShoulder)
                    AfterForwardGesture?.Invoke();
                else if (e.OnlyAlt && e.VirtualKey == Windows.System.VirtualKey.Right)
                    AfterForwardGesture?.Invoke();
                else
                    e.Handled = false;
            };
            _helper.GoPointerBackGestured = () => AfterBackGesture?.Invoke();
            _helper.PointerGoForwardGestured = () => AfterForwardGesture?.Invoke();
        }

        public Action AfterBackGesture { get; set; }
        public Action AfterForwardGesture { get; set; }
        public Action AfterControlEGesture { get; set; }
        public Action AfterWindowZGesture { get; set; }
    }

}
