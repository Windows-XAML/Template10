using System.Threading.Tasks;

namespace Template10.Service.Network
{
    public interface INetworkService 
    {
        Task<bool> GetIsInternetAvailableAsync();
        Task<bool> GetIsNetworkAvailableAsync();
    }
}