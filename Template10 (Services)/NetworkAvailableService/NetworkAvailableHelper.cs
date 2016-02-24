using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace Template10.Services.NetworkAvailableService
{
    public class NetworkAvailableHelper
    {
        public NetworkAvailableHelper()
        {
            NetworkInformation.NetworkStatusChanged += async (s) =>
            {
                var available = await this.IsInternetAvailable();
                if (AvailabilityChanged != null)
                {
                    try { AvailabilityChanged(available); }
                    catch { }
                }
            };
        }

        public Action<bool> AvailabilityChanged { get; set; }

        public Task<bool> IsInternetAvailable()
        {
            var _Profile = Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile();
            if (_Profile == null)
                return Task.FromResult<bool>(false);
            var net = Windows.Networking.Connectivity.NetworkConnectivityLevel.InternetAccess;
            return Task.FromResult<bool>(_Profile.GetNetworkConnectivityLevel().Equals(net));
        }
    }
}
