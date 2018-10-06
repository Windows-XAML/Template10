using System;
using System.Threading.Tasks;

using Windows.System;

namespace Template10.Services.Marketplace
{
    //https://msdn.microsoft.com/en-us/windows/uwp/launch-resume/launch-store-app
    public class MarketplaceHelper
    {
        public Uri AppInStoreByProductFamilyNameUri(string pfn)
        {
            return new Uri($"ms-windows-store:pdp?PFN={Uri.EscapeUriString(pfn)}");
        }

        public async Task LaunchAppInStoreByProductFamilyName(string pfn)
        {
            await Launcher.LaunchUriAsync(AppInStoreByProductFamilyNameUri(pfn));
        }

        public Uri AppInStoreByAppIdUri(string productId)
        {
            return new Uri($"ms-windows-store:pdp?ProductId={Uri.EscapeUriString(productId)}");
        }

        public async Task LaunchAppInStoreByAppId(string productId)
        {
            await Launcher.LaunchUriAsync(AppInStoreByAppIdUri(productId));
        }

        public Uri AppReviewInStoreByProductFamilyNameUri(string pfn)
        {
            return new Uri($"ms-windows-store:review?PFN={Uri.EscapeUriString(pfn)}");
        }

        public async Task LaunchAppReviewInStoreByProductFamilyName(string pfn)
        {
            await Launcher.LaunchUriAsync(AppReviewInStoreByProductFamilyNameUri(pfn));
        }

        public Uri AppReviewInStoreByAppIdUri(string productId)
        {
            return new Uri($"ms-windows-store:review?ProductId={Uri.EscapeUriString(productId)}");
        }

        public async Task LaunchAppReviewInStoreByAppId(string productId)
        {
            await Launcher.LaunchUriAsync(AppReviewInStoreByAppIdUri(productId));
        }

        public Uri PublisherPageInStoreUri(string publisherName)
        {
            return new Uri($"ms-windows-store:publisher?name={Uri.EscapeUriString(publisherName)}");
        }

        public async Task LaunchPublisherPageInStore(string publisherName)
        {
            await Launcher.LaunchUriAsync(PublisherPageInStoreUri(publisherName));
        }
    }
}
