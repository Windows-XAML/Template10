using System;
using System.Threading.Tasks;

namespace Template10.Services.NetworkAvailableService
{
    public interface INetworkAvailableService
    {
        Task<bool> IsInternetAvailable();
        Task<bool> IsNetworkAvailable();
        Action<ConnectionTypes> AvailabilityChanged { get; set; }
    }
}