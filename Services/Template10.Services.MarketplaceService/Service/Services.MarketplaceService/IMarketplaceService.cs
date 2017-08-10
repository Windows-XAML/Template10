using System.Threading.Tasks;
using Template10.Services.Nag;
using Template10.Services.Dialog;

namespace Template10.Services.Marketplace
{
    public interface IMarketplaceService
    {
        Task LaunchAppInStore();

        Task LaunchAppReviewInStoreAsync();

        Task LaunchPublisherPageInStoreAsync();

        NagObject CreateAppReviewNag();

        NagObject CreateAppReviewNag(string message);
    }
}
