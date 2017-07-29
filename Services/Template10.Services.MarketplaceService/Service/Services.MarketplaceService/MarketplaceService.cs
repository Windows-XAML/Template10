using System;
using System.Threading.Tasks;
using Template10.Services.NagService;
using Windows.ApplicationModel;

namespace Template10.Services.MarketplaceService
{
    public sealed class MarketplaceService : IMarketplaceService
    {
        public static MarketplaceService Create()
        {
            return new MarketplaceService();
        }

        readonly MarketplaceHelper _helper;
        private MarketplaceService()
        {
            _helper = new MarketplaceHelper();
        }

        public async Task LaunchAppInStore() =>
            await _helper.LaunchAppInStoreByProductFamilyName(Package.Current.Id.FamilyName);

        public async Task LaunchAppReviewInStoreAsync() =>
            await _helper.LaunchAppReviewInStoreByProductFamilyName(Package.Current.Id.FamilyName);

        public async Task LaunchPublisherPageInStoreAsync() =>
            await _helper.LaunchPublisherPageInStore(Package.Current.Id.Publisher);

        public Nag CreateAppReviewNag() => CreateAppReviewNag($"Review {Package.Current.DisplayName}?");

        public Nag CreateAppReviewNag(string message)
        {
            return new Nag("AppReviewNag", message, async () => await this.LaunchAppReviewInStoreAsync())
            {
                Title = $"Review {Package.Current.DisplayName}",
                AcceptText = "Review app",
                Location = Nag.StorageStrategies.Roaming
            };
        }
    }
}
