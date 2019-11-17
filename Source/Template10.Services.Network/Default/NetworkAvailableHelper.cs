using System;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace Template10.Services
{
    public class NetworkAvailableHelper
    {
        public NetworkAvailableHelper()
        {
            NetworkInformation.NetworkStatusChanged += (s) =>
            {
                var internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
                var conn = ConnectionTypes.None;
                if (internetConnectionProfile != null)
                {
                    var connectionLevel = internetConnectionProfile.GetNetworkConnectivityLevel();
                    conn = (connectionLevel == NetworkConnectivityLevel.InternetAccess) ? ConnectionTypes.Internet : ConnectionTypes.LocalNetwork;
                }
                if (AvailabilityChanged != null)
                {
                    try { AvailabilityChanged(conn); }
                    catch { }
                }
            };
        }

        public Action<ConnectionTypes> AvailabilityChanged { get; set; }

        public Task<bool> GetIsInternetAvailableAsync()
        {
            var _Profile = NetworkInformation.GetInternetConnectionProfile();
            if (_Profile == null)
            {
                return Task.FromResult<bool>(false);
            }

            var net = NetworkConnectivityLevel.InternetAccess;
            return Task.FromResult<bool>(_Profile.GetNetworkConnectivityLevel().Equals(net));
        }

        public Task<bool> GetIsNetworkAvailableAsync()
        {
            var _Profile = NetworkInformation.GetInternetConnectionProfile();
            if (_Profile == null)
            {
                return Task.FromResult<bool>(false);
            }

            var local = NetworkConnectivityLevel.LocalAccess;
            return Task.FromResult<bool>(_Profile.GetNetworkConnectivityLevel().Equals(local));
        }
    }
}
