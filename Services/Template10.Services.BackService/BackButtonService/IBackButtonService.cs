using System;
using System.ComponentModel;
using Template10.Common;

namespace Template10.Services.Gesture
{
    public interface IBackButtonService
    {
        event EventHandler BackButtonUpdated;
        event TypedEventHandler<HandledEventArgs> BackRequested;
        event TypedEventHandler<CancelEventArgs> BeforeBackRequested;
        event TypedEventHandler<CancelEventArgs> BeforeForwardRequested;
        event TypedEventHandler<HandledEventArgs> ForwardRequested;
        void UpdateBackButton(bool canGoBack, bool canGoForward = false);
    }
}