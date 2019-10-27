using System;

namespace Template10.Services
{
    public class AvailabilityChangedEventArgs : EventArgs
    {
        public ConnectionTypes ConnectionType { get; set; }
    }
}
