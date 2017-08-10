using System;
using System.Threading.Tasks;

namespace Template10.Services.Network
{
    public abstract class NetworkAvailableServiceBase
    {
        public NetworkAvailableServiceBase() => Helper = new NetworkAvailableHelper();

        public NetworkAvailableHelper Helper { get; private set; }
    }

    public class NetworkAvailableService : NetworkAvailableServiceBase, INetworkAvailableService
    {
        public async Task<bool> IsInternetAvailable() => await Helper.IsInternetAvailable();

        public async Task<bool> IsNetworkAvailable() => await Helper.IsNetworkAvailable();

        public Action<ConnectionTypes> AvailabilityChanged
        {
            get => Helper.AvailabilityChanged;
            set => Helper.AvailabilityChanged = value;
        }
    }
}
