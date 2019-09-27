using System;

namespace Template10.Service.Network
{
    public class AvailabilityChangedEventArgs : EventArgs
    {
        public ConnectionTypes ConnectionType { get; set; }
    }
}
