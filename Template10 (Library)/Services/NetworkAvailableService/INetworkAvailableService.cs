using System;
using System.Threading.Tasks;

namespace Template10.Services.NetworkAvailableService
{
    public interface INetworkAvailableService
    {
        Action<bool> AvailabilityChanged { get; set; }
        Task<bool> IsInternetAvailable();
    }
}