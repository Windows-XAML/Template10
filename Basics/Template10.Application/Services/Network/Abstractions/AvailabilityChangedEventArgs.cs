using System;

namespace Prism.Windows.Services.Network
{
    public class AvailabilityChangedEventArgs : EventArgs
    {
        public ConnectionTypes ConnectionType { get; set; }
    }
}
