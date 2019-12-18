using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Template10.Services
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

        public async Task<ContentDialogResult> ShowAsync(ContentDialog dialog, TimeSpan? timeout = null, CancellationToken? token = null)
        {
            return await DialogManager.OneAtATimeAsync(async () => await dialog.ShowAsync(), timeout, token);
        }

        public async Task<IUICommand> ShowAsync(MessageDialog dialog, TimeSpan? timeout = null, CancellationToken? token = null)
        {
            return await DialogManager.OneAtATimeAsync(async () => await dialog.ShowAsync(), timeout, token);
        }
    }
}
