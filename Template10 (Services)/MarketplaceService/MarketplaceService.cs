using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.ApplicationModel;

namespace Template10.Services.MarketplaceService
{
    public sealed class MarketplaceService : IMarketplaceService
    {
        readonly MarketplaceHelper _helper = new MarketplaceHelper();

        public async Task LaunchAppInStore() => 
            await _helper.LaunchAppInStoreByProductFamilyName(Package.Current.Id.FamilyName);        

        public async Task LaunchAppReviewInStore() => 
            await _helper.LaunchAppReviewInStoreByProductFamilyName(Package.Current.Id.FamilyName);        

        public async Task LaunchPublisherPageInStore() =>
            await _helper.LaunchPublisherPageInStore(Package.Current.Id.Publisher);        
    }
}
