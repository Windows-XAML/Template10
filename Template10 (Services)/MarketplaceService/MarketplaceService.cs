using System.Threading.Tasks;

using Windows.ApplicationModel;

using Template10.Services.NagService;
using Template10.Services.FileService;

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

        public Nag CreateAppReviewNag() => CreateAppReviewNag($"Would you mind reviewing {Package.Current.DisplayName}?");

        public Nag CreateAppReviewNag(string message)
        {
            return new Nag("AppReviewNag", message, async () => await this.LaunchAppReviewInStore())
            {
                Title = $"Review {Package.Current.DisplayName}",
                AcceptText = "Review app",
                Location = StorageStrategies.Roaming
            };
        }
    }
}
