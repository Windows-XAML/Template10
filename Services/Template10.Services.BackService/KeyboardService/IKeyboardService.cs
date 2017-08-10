using System;
using Template10.Common;
using Windows.Foundation;

namespace Template10.Services.Gesture
{
    public interface IKeyboardService
    {
        void Setup();
        event EventHandler AfterSearchGesture;
        event EventHandler AfterMenuGesture;
        event TypedEventHandler<KeyboardEventArgs> AfterKeyDown;
    }
}