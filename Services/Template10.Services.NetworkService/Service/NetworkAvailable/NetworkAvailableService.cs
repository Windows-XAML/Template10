using System;
using System.Threading.Tasks;

namespace Template10.Services.Network
{
    public class NetworkAvailableService : INetworkAvailableService
    {
        public NetworkAvailableService()
        {
            Helper = new NetworkAvailableHelper();
            Helper.AvailabilityChanged += (e) 
                => AvailabilityChanged?.Invoke(this, new AvailabilityChangedEventArgs { ConnectionType = e });
        }

        public NetworkAvailableHelper Helper { get; private set; }

        public async Task<bool> GetIsInternetAvailableAsync()
            => await Helper.GetIsInternetAvailableAsync();

        public async Task<bool> GetIsNetworkAvailableAsync()
            => await Helper.GetIsNetworkAvailableAsync();

        public event EventHandler<AvailabilityChangedEventArgs> AvailabilityChanged;
    }

    public class AvailabilityChangedEventArgs : EventArgs
    {
        public ConnectionTypes ConnectionType { get; set; }
    }
}
