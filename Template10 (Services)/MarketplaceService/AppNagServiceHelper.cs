using System;
using System.Threading.Tasks;

using Windows.UI.Popups;
using Windows.ApplicationModel;

using Template10.Services.DialogService;
using Template10.Services.FileService;

namespace Template10.Services.MarketplaceService
{
    public class AppNagServiceHelper
    {
        readonly IDialogService _dialogService;
        readonly IFileService _fileService;
        readonly IMarketplaceService _marketplaceService;

        NagInfo _nagInfo;

        const string StateFileKey = "Template10.Services.MarketplaceService.AppReview.json";

        public AppNagServiceHelper(IDialogService dialogService, IFileService fileService, IMarketplaceService marketplaceService)
        {
            _dialogService = dialogService;
            _fileService = fileService;
            _marketplaceService = marketplaceService;
        }

        public async Task<bool> Initialize()
        {
            try
            {
                if (await _fileService.FileExistsAsync(StateFileKey, StorageStrategies.Roaming))
                {
                    _nagInfo = await _fileService.ReadFileAsync<NagInfo>(StateFileKey, StorageStrategies.Roaming);
                }
                else
                {
                    _nagInfo = new NagInfo() { FirstRegistered = DateTimeOffset.UtcNow };
                }
            }
            catch
            {
                _nagInfo = new NagInfo() { Suppress = true };
            }

            _nagInfo.LaunchCount++;
            return !_nagInfo.Suppress;
        }

        public async Task ShowNag(string messageTemplate, string titleTemplate, string yesButtonText, string noButtonText)
        {
            string name = Package.Current.DisplayName;
            await _dialogService.ShowAsync(string.Format(messageTemplate, name), string.Format(titleTemplate, name),
                new UICommand(yesButtonText, command => _marketplaceService.LaunchAppReviewInStore()),
                new UICommand(noButtonText)
                );

            _nagInfo.Suppress = true;
        }

        public bool ShouldNag(int launches)
        {
            return launches < 1 || _nagInfo.LaunchCount > launches;
        }

        public bool ShouldNag(TimeSpan duration)
        {
            return duration <= TimeSpan.Zero || DateTimeOffset.UtcNow + duration > _nagInfo.FirstRegistered;
        }

        public async Task PersistState()
        {
            await _fileService.WriteFileAsync<NagInfo>(StateFileKey, _nagInfo, StorageStrategies.Roaming);
        }
    }
}
