using System.Threading.Tasks;

namespace Template10.Services.NetworkAvailableService
{
    public class NetworkAvailableService : INetworkAvailableService
    {
        NetworkAvailableHelper _helper = new NetworkAvailableHelper();

        public async Task<bool> IsInternetAvailable() => await _helper.IsInternetAvailable();
    }
}
