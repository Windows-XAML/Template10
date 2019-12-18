using System.Threading.Tasks;

namespace Template10.Services.Marketplace
{
    public interface IMarketplaceService
    {
        Task LaunchAppInStore();

        Task LaunchAppReviewInStoreAsync();

        Task LaunchPublisherPageInStoreAsync();

        NagEx CreateAppReviewNag();

        NagEx CreateAppReviewNag(string message);
    }
}
