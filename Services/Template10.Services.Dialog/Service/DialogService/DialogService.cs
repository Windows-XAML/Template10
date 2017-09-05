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
            return await new MessageBoxEx(content, MessageBoxType.Ok, resolver).QueueAsync();
        }

        public async Task<MessageBoxResult> PromptAsync(string content, MessageBoxType type = MessageBoxType.YesNo, IResourceResolver resolver = null)
        {
            return await new MessageBoxEx(content, type, resolver).QueueAsync();
        }
    }
}
