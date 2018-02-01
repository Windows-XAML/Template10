using System.Threading.Tasks;

namespace Prism.Windows.Services.Network
{
    public interface INetworkService 
    {
        Task<bool> GetIsInternetAvailableAsync();
        Task<bool> GetIsNetworkAvailableAsync();
    }
}