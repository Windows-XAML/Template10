using System.Threading.Tasks;
using Template10.Services.Nag;

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
