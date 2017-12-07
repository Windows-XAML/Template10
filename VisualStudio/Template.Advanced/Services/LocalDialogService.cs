using System;
using System.Threading.Tasks;
using Template10.Services.Dialog;
using Template10.Services.Resources;

namespace Sample.Services
{
    public class LocalDialogService : ILocalDialogService
    {
        IDialogService _dialogService;
        IResourceService _resourceService;
        IDialogResourceResolver _resolver;

        public LocalDialogService(IDialogService dialogService, IResourceService resources)
        {
            _resourceService = resources;
            _dialogService = dialogService;
            _resolver = new LocalDialogResourceResolver(resources);
        }

        public async Task<bool> ShowAreYouSureAsync()
        {
            var result = await _dialogService.PromptAsync(AreYouSure_Title, AreYouSure_Content, MessageBoxType.YesNo, _resolver);
            return result.Equals(MessageBoxResult.Yes);
        }

        string AreYouSure_Title => _resourceService.GetLocalizedString("AreYouSure_Title");
        string AreYouSure_Content => _resourceService.GetLocalizedString("AreYouSure_Content");
    }
}
