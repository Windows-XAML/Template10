using System;
using System.Threading.Tasks;

namespace Template10.Services.Network
{
    public interface INetworkAvailableService
    {
        Task<bool> GetIsInternetAvailableAsync();
        Task<bool> GetIsNetworkAvailableAsync();
        event EventHandler<AvailabilityChangedEventArgs> AvailabilityChanged;
    }
}