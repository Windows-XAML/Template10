using System;
using System.Threading.Tasks;

namespace Template10.Services.InAppPurchaseService
{
    class InAppPurchaseService
    {
        InAppPurchaseHelper _helper;

        public InAppPurchaseService()
        {
            _helper = new InAppPurchaseHelper("RemoveAds", true);
        }

        public bool IsPurchased()
        {
            return _helper?.IsPurchased ?? false;
        }

        public async Task<bool> PurchaseAsync()
        {
            await _helper.SetupAsync();
            return await _helper.Purchase();
        }
    }
}
