using System.Threading.Tasks;
using Template10.Service.Nag;

namespace Template10.Service.Marketplace
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
