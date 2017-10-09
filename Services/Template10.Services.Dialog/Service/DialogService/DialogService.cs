using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Extensions;
using Windows.UI.Popups;
using static Template10.Services.Dialog.MessageBoxEx;

namespace Template10.Services.Dialog
{
    public class DialogService : IDialogService
    {
        public async Task<MessageBoxResult> AlertAsync(string content, IResourceResolver resolver = null)
            => await AlertAsync(string.Empty, content, resolver);

        public async Task<MessageBoxResult> AlertAsync(string title, string content, IResourceResolver resolver = null)
            => await new MessageBoxEx(title, content, MessageBoxType.Ok, resolver).ShowAsync();

        public async Task<MessageBoxResult> PromptAsync(string content, MessageBoxType type = MessageBoxType.YesNo, IResourceResolver resolver = null)
            => await PromptAsync(string.Empty, content, type, resolver);

        public async Task<MessageBoxResult> PromptAsync(string title, string content, MessageBoxType type = MessageBoxType.YesNo, IResourceResolver resolver = null)
            => await new MessageBoxEx(title, content, type, resolver).ShowAsync();

        public async Task<bool> PromptAsync(string content, MessageBoxType type, MessageBoxResult expected, IResourceResolver resolver = null)
            => await PromptAsync(string.Empty, content, type, expected, resolver);

        public async Task<bool> PromptAsync(string title, string content, MessageBoxType type, MessageBoxResult expected, IResourceResolver resolver = null)
            => (await PromptAsync(title, content, type, resolver)).Equals(expected);
    }
}
