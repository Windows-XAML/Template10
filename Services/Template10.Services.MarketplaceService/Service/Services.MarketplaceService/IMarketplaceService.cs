using System.Threading.Tasks;

using Template10.Services.NagService;

namespace Template10.Services.MarketplaceService
{
    public interface IMarketplaceService
    {
        Task LaunchAppInStore();

        Task LaunchAppReviewInStoreAsync();

        Task LaunchPublisherPageInStoreAsync();

        Nag CreateAppReviewNag();

        Nag CreateAppReviewNag(string message);
    }
}
