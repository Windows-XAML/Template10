using System;
using System.Threading.Tasks;
using Template10.Services.Dialog;
using Template10.Services.Resources;

namespace Sample.Services
{
    public class LocalDialogService : ILocalDialogService
    {
        IDialogService _dialogService;
        IResourceService _resources;
        public LocalDialogService(IDialogService dialogService, IResourceService resources)
        {
            _dialogService = dialogService;
            _resources = resources;
        }

        public async Task<bool> AreYouSureAsync()
        {
            (string Title, string Content, IResourceResolver Resolver) resource = (
                _resources.GetLocalizedString("AreYouSure_Title"),
                _resources.GetLocalizedString("AreYouSure_Content"),
                new EmptyResourceResolver
                {
                    Resolve = t =>
                    {
                        switch (t)
                        {
                            case ResourceTypes.Yes: return _resources.GetLocalizedString("AreYouSure_Button1Text");
                            case ResourceTypes.No: return _resources.GetLocalizedString("AreYouSure_Button2Text");
                            default: throw new NotImplementedException();
                        }
                    }
                });
            var result = await _dialogService.PromptAsync(resource.Title, resource.Content, MessageBoxType.YesNo, resource.Resolver);
            return result.Equals(MessageBoxResult.Yes);
        }
    }
}
