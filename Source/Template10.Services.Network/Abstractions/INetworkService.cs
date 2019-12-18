using System.Threading.Tasks;

namespace Template10.Services
{
    public interface INetworkService
    {
        Task<bool> GetIsInternetAvailableAsync();
        Task<bool> GetIsNetworkAvailableAsync();
    }
}