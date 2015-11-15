using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace Template10.Services.NetworkAvailableService
{
    public interface INetworkAvailableService
    {
        System.Threading.Tasks.Task<bool> IsInternetAvailable();
    }
}