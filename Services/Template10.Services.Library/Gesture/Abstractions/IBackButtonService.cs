using System;
using System.ComponentModel;
using Windows.Foundation;

namespace Template10.Services.Gesture
{
    public interface IBackButtonService
    {
        event EventHandler BackButtonUpdated;
        event TypedEventHandler<object, HandledEventArgs> BackRequested;
        event TypedEventHandler<object, CancelEventArgs> BeforeBackRequested;
        event TypedEventHandler<object, CancelEventArgs> BeforeForwardRequested;
        event TypedEventHandler<object, HandledEventArgs> ForwardRequested;
        void UpdateBackButton(bool canGoBack, bool canGoForward = false);
    }
}