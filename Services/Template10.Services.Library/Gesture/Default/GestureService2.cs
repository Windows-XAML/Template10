using System;
using Windows.Foundation;

namespace Template10.Services.Gesture
{
    public partial class GestureService : IGestureService2
    {
        IGestureService2 Two => this as IGestureService2;

        event EventHandler _backRequested;
        event EventHandler IGestureService2.BackRequested
        {
            add => _backRequested += value;
            remove => _backRequested -= value;
        }

        event EventHandler _forwardRequested;
        event EventHandler IGestureService2.ForwardRequested
        {
            add => _forwardRequested += value;
            remove => _forwardRequested -= value;
        }

        event TypedEventHandler<object, KeyboardEventArgs> _afterKeyDown;
        event TypedEventHandler<object, KeyboardEventArgs> IGestureService2.AfterKeyDown
        {
            add => _afterKeyDown += value;
            remove => _afterKeyDown -= value;
        }
    }
}