using System;
using Windows.Foundation;

namespace Template10.Services.KeyboardService
{
    public interface IKeyboardService
    {
        void Setup();
        Action AfterControlEGesture { get; set; }
        Action AfterMenuGesture { get; set; }
        Action<KeyboardEventArgs> AfterKeyDown { get; set; }
    }
}