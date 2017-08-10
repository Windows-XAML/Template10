using System;
using System.Threading.Tasks;

namespace Template10.Services.Network
{
    public interface INetworkAvailableService
    {
        Task<bool> IsInternetAvailable();
        Task<bool> IsNetworkAvailable();
        Action<ConnectionTypes> AvailabilityChanged { get; set; }
    }
}