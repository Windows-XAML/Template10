using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.System;

namespace Template10.Services.MarketplaceService
{
    //https://msdn.microsoft.com/en-us/windows/uwp/launch-resume/launch-store-app
    public class MarketplaceHelper
    {
        public async Task LaunchAppInStoreByProductFamilyName(string pfn)
        {
            await Launcher.LaunchUriAsync(new Uri($"ms-windows-store:pdp?PFN={Uri.EscapeUriString(pfn)}"));
        }

        public async Task LaunchAppInStoreByAppId(string productId)
        {
            await Launcher.LaunchUriAsync(new Uri($"ms-windows-store:pdp?ProductId={Uri.EscapeUriString(productId)}"));
        }

        public async Task LaunchAppReviewInStoreByProductFamilyName(string pfn)
        {
            await Launcher.LaunchUriAsync(new Uri($"ms-windows-store:review?PFN={Uri.EscapeUriString(pfn)}"));
        }

        public async Task LaunchAppReviewInStoreByAppId(string productId)
        {
            await Launcher.LaunchUriAsync(new Uri($"ms-windows-store:review?ProductId={Uri.EscapeUriString(productId)}"));
        }

        public async Task LaunchPublisherPageInStore(string publisherName)
        {
            await Launcher.LaunchUriAsync(new Uri($"ms-windows-store:publisher?name={Uri.EscapeUriString(publisherName)}"));
        }
    }
}
