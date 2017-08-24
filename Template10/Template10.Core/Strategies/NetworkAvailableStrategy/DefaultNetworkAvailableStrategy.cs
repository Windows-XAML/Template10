using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Services.Network;

namespace Template10.Strategies
{
    public enum NetworkRequirements { None, NetworkRequired, InternetRequired }

    public static partial class Settings
    {
        public static NetworkRequirements NetworkRequirement { get; set; } = NetworkRequirements.None;
    }

    public interface IDefaultNetworkAvailableStrategy
    {
    }

    public class DefaultNetworkAvailableStrategy : IDefaultNetworkAvailableStrategy
    {
        private INetworkAvailableService _service;
        public DefaultNetworkAvailableStrategy(INetworkAvailableService service)
        {
            _service = service;
            _service.AvailabilityChanged += _service_AvailabilityChanged;
        }

        private void _service_AvailabilityChanged(object sender, AvailabilityChangedEventArgs e)
        {
            switch (Settings.NetworkRequirement)
            {
                case NetworkRequirements.None:
                    HandleCorrect(NetworkRequirements.None, e.ConnectionType);
                    break;
                case NetworkRequirements.NetworkRequired when (e.ConnectionType == ConnectionTypes.LocalNetwork | e.ConnectionType == ConnectionTypes.Internet):
                    HandleCorrect(NetworkRequirements.NetworkRequired, e.ConnectionType);
                    break;
                case NetworkRequirements.InternetRequired when (e.ConnectionType == ConnectionTypes.Internet):
                    HandleCorrect(NetworkRequirements.InternetRequired, e.ConnectionType);
                    break;
                default:
                    HandleIncorrect(Settings.NetworkRequirement, e.ConnectionType);
                    break;
            }
        }

        public void HandleCorrect(NetworkRequirements desired, ConnectionTypes actual)
        {
            // TODO
        }

        public void HandleIncorrect(NetworkRequirements desired, ConnectionTypes actual)
        {
            // TODO
        }
    }
}
