using System;
using System.Threading.Tasks;
using Template10.Services.Dialog;

namespace Sample.Services
{

    public class LocalDialogService : ILocalDialogService
    {
        IDialogService _dialogService;

        public LocalDialogService(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public async Task<bool> ShowAreYouSureAsync()
        {
            var title = "Continue";
            var content = "Are you sure?";
            var result = await _dialogService.PromptAsync(title, content, MessageBoxType.YesNo);
            return result.Equals(MessageBoxResult.Yes);
        }
    }
}
