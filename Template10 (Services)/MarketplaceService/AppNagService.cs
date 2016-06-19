using System;
using System.Threading.Tasks;

using Template10.Services.DialogService;
using Template10.Services.FileService;

namespace Template10.Services.MarketplaceService
{
    public class AppNagService : IAppNagService
    {
        readonly AppNagServiceHelper _helper;

        public AppNagService()
            : this(new DialogService.DialogService(), new FileService.FileService(), new MarketplaceService())
        {
        }

        public AppNagService(IDialogService dialogService, IFileService fileService, IMarketplaceService marketplaceService)
        {
            _helper = new AppNagServiceHelper(dialogService, fileService, marketplaceService);
        }

        public string MessageTemplate { get; set; } = "Would you mind reviewing {0}?";

        public string NoButtonText { get; set; } = "No, thanks";

        public string TitleTemplate { get; set; } = "Review {0}";

        public string YesButtonText { get; set; } = "Rate";

        public async Task RegisterAppReviewNag(TimeSpan duration)
        {
            if (await _helper.Initialize())
            {
                if (_helper.ShouldNag(duration))
                {
                    await _helper.ShowNag(MessageTemplate, TitleTemplate, YesButtonText, NoButtonText);
                    await _helper.PersistState();
                }
            }
        }

        public async Task RegisterAppReviewNag(int launches)
        {
            if (await _helper.Initialize())
            {
                if (_helper.ShouldNag(launches))
                {
                    await _helper.ShowNag(MessageTemplate, TitleTemplate, YesButtonText, NoButtonText);
                }

                await _helper.PersistState();
            }
        }

        public async Task RegisterAppReviewNag(int launches, TimeSpan duration)
        {
            if (await _helper.Initialize())
            {
                if (_helper.ShouldNag(launches) && _helper.ShouldNag(duration))
                {
                    await _helper.ShowNag(MessageTemplate, TitleTemplate, YesButtonText, NoButtonText);
                }

                await _helper.PersistState();
            }
        }
    }
}
