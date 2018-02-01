using System.Threading.Tasks;
using Prism.Windows.Services.Nag;

namespace Prism.Windows.Services.Marketplace
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
