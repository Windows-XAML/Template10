using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Template10.Services.Dialog
{
    public class DialogService : IDialogService
    {
        public async Task<MessageBoxResult> AlertAsync(string content, IDialogResourceResolver resolver = null)
            => await AlertAsync(string.Empty, content, resolver, ElementTheme.Light);
        
        public async Task<MessageBoxResult> AlertAsync(string title, string content, IDialogResourceResolver resolver = null, ElementTheme requestedTheme = ElementTheme.Light)
            => await new MessageBoxEx(title, content, MessageBoxType.Ok, resolver, requestedTheme).ShowAsync();
        
        public async Task<MessageBoxResult> PromptAsync(string content, MessageBoxType type = MessageBoxType.YesNo, IDialogResourceResolver resolver = null, 
			ElementTheme requestedTheme = ElementTheme.Light, string yesText = null, string noText = null)
            => await PromptAsync(string.Empty, content, type, resolver, requestedTheme, yesText, noText);

        public async Task<MessageBoxResult> PromptAsync(string title, string content, MessageBoxType type = MessageBoxType.YesNo, IDialogResourceResolver resolver = null, 
			ElementTheme requestedTheme = ElementTheme.Light, string yesText = null, string noText = null)
            => await new MessageBoxEx(title, content, type, resolver, requestedTheme, noText, yesText).ShowAsync();

        public async Task<bool> PromptAsync(string content, MessageBoxType type, MessageBoxResult expected, IDialogResourceResolver resolver = null, 
			ElementTheme requestedTheme = ElementTheme.Light, string yesText = null, string noText = null)
            => await PromptAsync(string.Empty, content, type, expected, resolver, requestedTheme);

        public async Task<bool> PromptAsync(string title, string content, MessageBoxType type, MessageBoxResult expected, IDialogResourceResolver resolver = null, 
			ElementTheme requestedTheme = ElementTheme.Light, string yesText = null, string noText = null)
            => (await PromptAsync(title, content, type, resolver, requestedTheme)).Equals(expected);
    }
}
