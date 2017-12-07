using System;
using Windows.Foundation;

namespace Template10.Services.Gesture
{
    public enum EventModes { Block, Allow }

    public interface IGestureService
    {
        EventModes BackRequestedMode { get; set; }
        EventModes BackForwardRequestedMode { get; set; }
    }
}