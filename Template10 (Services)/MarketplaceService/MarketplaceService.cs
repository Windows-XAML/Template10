using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.MarketplaceService
{
    public class MarketplaceService : IMarketplaceService
    {
        MarketplaceHelper _helper = new MarketplaceHelper();

        public void LaunchAppInStore()
        {
            _helper.LaunchAppInStore();
        }
    }
}
