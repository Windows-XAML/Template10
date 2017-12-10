using System;
using Windows.Foundation;

namespace Template10.Services.Gesture
{
    public enum EventModes { Block, Allow }

    public interface IGestureService
    {
        event EventHandler BackButtonUpdated;
        EventModes BackRequestedMode { get; set; }
        EventModes BackForwardRequestedMode { get; set; }
        void UpdateBackButton(bool canGoBack, bool canGoForward = false);
    }
}