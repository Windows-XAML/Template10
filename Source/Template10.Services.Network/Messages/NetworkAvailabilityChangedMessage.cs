using Template10.Services;

namespace Template10.Messages
{
    public class NetworkAvailabilityChangedMessage
    {
        public NetworkAvailabilityChangedMessage(ConnectionTypes connectionType)
        {
            ConnectionType = connectionType;
        }

        public ConnectionTypes ConnectionType { get; set; }
    }
}
