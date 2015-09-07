using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace Template10.Services.NetworkAvailableService
{
    public class NetworkAvailableService
    {
        NetworkAvailableHelper _helper = new NetworkAvailableHelper();

        public async Task<bool> IsInternetAvailable()
        {
            return await _helper.IsInternetAvailable();
        }
    }
}
