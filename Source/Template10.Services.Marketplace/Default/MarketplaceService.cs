using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace Template10.Services.Marketplace
{
    public sealed class MarketplaceService : IMarketplaceService
    {
        private readonly MarketplaceHelper _helper;
        public MarketplaceService()
        {
            _helper = new MarketplaceHelper();
        }

        public async Task LaunchAppInStore() =>
            await _helper.LaunchAppInStoreByProductFamilyName(Package.Current.Id.FamilyName);

        public async Task LaunchAppReviewInStoreAsync() =>
            await _helper.LaunchAppReviewInStoreByProductFamilyName(Package.Current.Id.FamilyName);

        public async Task LaunchPublisherPageInStoreAsync() =>
            await _helper.LaunchPublisherPageInStore(Package.Current.Id.Publisher);

        public NagEx CreateAppReviewNag() => CreateAppReviewNag($"Review {Package.Current.DisplayName}?");

        public NagEx CreateAppReviewNag(string message)
        {
            return new NagEx("AppReviewNag", message, async () => await LaunchAppReviewInStoreAsync())
            {
                Title = $"Review {Package.Current.DisplayName}",
                AcceptText = "Review app",
                Location = NagStorageStrategies.Roaming
            };
        }
    }
}
