using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Messages
{
    public class NetworkAvailabilityChangedMessage
    {
        public NetworkAvailabilityChangedMessage(Services.Network.ConnectionTypes connectionType)
        {
            ConnectionType = connectionType;
        }

        public Services.Network.ConnectionTypes ConnectionType { get; set; }
    }
}
