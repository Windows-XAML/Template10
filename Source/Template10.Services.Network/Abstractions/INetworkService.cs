using System.Threading.Tasks;

namespace Template10.Services.Network
{
    public interface INetworkService 
    {
        Task<bool> GetIsInternetAvailableAsync();
        Task<bool> GetIsNetworkAvailableAsync();
    }
}