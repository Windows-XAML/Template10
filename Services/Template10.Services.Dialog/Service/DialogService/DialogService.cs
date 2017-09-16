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
        public Func<ResourceTypes, string> ResolveResource { get; set; }

        public async Task<MessageBoxResult> AlertAsync(string content, IResourceResolver resolver = null)
        {
            return await new MessageBoxEx(content, MessageBoxType.Ok, resolver).ShowAsync();
        }

        public async Task<MessageBoxResult> PromptAsync(string content, MessageBoxType type = MessageBoxType.YesNo, IResourceResolver resolver = null)
        {
            return await new MessageBoxEx(content, type, resolver).ShowAsync();
        }

        public async Task<bool> PromptAsync(string content, MessageBoxType type, MessageBoxResult expected, IResourceResolver resolver = null)
        {
            return (await PromptAsync(content, type, resolver)).Equals(expected);
        }
    }
}
