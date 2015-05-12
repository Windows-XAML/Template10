using System;

namespace Blank1.Services.KeyboardService
{
    public class KeyboardService
    {
        KeyboardHelper _helper;

        public KeyboardService()
        {
            _helper = new KeyboardHelper();
            _helper.GoBackGestured = () => { AfterBackGesture?.Invoke(); };
            _helper.GoForwardGestured = () => { AfterForwardGesture?.Invoke(); };
        }

        public Action AfterBackGesture { get; set; }
        public Action AfterForwardGesture { get; set; }
    }

}
