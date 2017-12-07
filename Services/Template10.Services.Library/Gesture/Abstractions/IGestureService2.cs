using System;
using Windows.Foundation;

namespace Template10.Services.Gesture
{
    public interface IGestureService2
    {
        void Setup();
        event EventHandler BackRequested;
        event EventHandler ForwardRequested;
        event TypedEventHandler<object, KeyboardEventArgs> AfterKeyDown;
    }
}