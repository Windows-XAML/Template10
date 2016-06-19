using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Template10.Mvvm;
using Template10.Services.MarketplaceService;

namespace Template10.Samples.Marketplace.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        // in a publisher app you would use the MarketplaceService NOT the marketplace helper
        // but since the sample is unpublished it doesn't have id's that are in the store
        //readonly IMarketplaceService _marketplaceService = new MarketplaceService();

        // this and its usage are just for testing purposes - in a published app the MarketplaceService
        // uses the app pacakages id's so they don't need to be supplied by the app
        readonly MarketplaceHelper _marketplaceHelper = new MarketplaceHelper();

        DelegateCommand<string> _ShowPublisherCommand;
        public DelegateCommand<string> ShowPublisherCommand => _ShowPublisherCommand ??
            (
                _ShowPublisherCommand = new DelegateCommand<string>(publisher => _marketplaceHelper.LaunchPublisherPageInStore(publisher))
            );

        DelegateCommand<string> _ShowAppInStore;
        public DelegateCommand<string> ShowAppInStore => _ShowAppInStore ??
            (
                _ShowAppInStore = new DelegateCommand<string>(pfn => _marketplaceHelper.LaunchAppInStoreByProductFamilyName(pfn))
            );

        DelegateCommand<string> _ShowAppReviewInStore;
        public DelegateCommand<string> ShowAppReviewInStore => _ShowAppReviewInStore ??
            (
                _ShowAppReviewInStore = new DelegateCommand<string>(pfn => _marketplaceHelper.LaunchAppReviewInStoreByProductFamilyName(pfn))
            );
    }
}
