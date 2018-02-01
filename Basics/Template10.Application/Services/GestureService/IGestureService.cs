using System;
using Windows.Foundation;

namespace Prism.Windows.Services
{
    public interface IGestureService
    {
        event EventHandler BackRequested;
        event EventHandler ForwardRequested;
        event TypedEventHandler<object, KeyDownEventArgs> KeyDown;
        event EventHandler MenuRequested;
        event EventHandler RefreshRequested;
        event EventHandler SearchRequested;
    }
}