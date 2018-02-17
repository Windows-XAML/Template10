using System.Threading.Tasks;

namespace Template10.Services.Dialog
{
    public class DialogService : IDialogService
    {
        public async Task<MessageBoxResult> AlertAsync(string content, IDialogResourceResolver resolver = null)
            => await AlertAsync(string.Empty, content, resolver);

        public async Task<MessageBoxResult> AlertAsync(string title, string content, IDialogResourceResolver resolver = null)
            => await new MessageBoxEx(title, content, MessageBoxType.Ok, resolver).ShowAsync();

        public async Task<MessageBoxResult> PromptAsync(string content, MessageBoxType type = MessageBoxType.YesNo, IDialogResourceResolver resolver = null)
            => await PromptAsync(string.Empty, content, type, resolver);

        public async Task<MessageBoxResult> PromptAsync(string title, string content, MessageBoxType type = MessageBoxType.YesNo, IDialogResourceResolver resolver = null)
            => await new MessageBoxEx(title, content, type, resolver).ShowAsync();

        public async Task<bool> PromptAsync(string content, MessageBoxType type, MessageBoxResult expected, IDialogResourceResolver resolver = null)
            => await PromptAsync(string.Empty, content, type, expected, resolver);

        public async Task<bool> PromptAsync(string title, string content, MessageBoxType type, MessageBoxResult expected, IDialogResourceResolver resolver = null)
            => (await PromptAsync(title, content, type, resolver)).Equals(expected);
    }
}
