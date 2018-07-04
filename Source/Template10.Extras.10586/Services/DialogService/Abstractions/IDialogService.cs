using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Template10.Services.Dialog
{
    public interface IDialogService
    {
        Task<MessageBoxResult> AlertAsync(string content, IDialogResourceResolver resolver = null);

        Task<MessageBoxResult> AlertAsync(string title, string content, IDialogResourceResolver resolver = null, ElementTheme requestedTheme = ElementTheme.Light);

        Task<MessageBoxResult> PromptAsync(string content, MessageBoxType type = MessageBoxType.YesNo, IDialogResourceResolver resolver = null, 
			ElementTheme requestedTheme = ElementTheme.Light, string yesText = null, string noText = null);

        Task<MessageBoxResult> PromptAsync(string title, string content, MessageBoxType type = MessageBoxType.YesNo, IDialogResourceResolver resolver = null, 
			ElementTheme requestedTheme = ElementTheme.Light, string yesText = null, string noText = null);

        Task<bool> PromptAsync(string content, MessageBoxType type, MessageBoxResult expected, IDialogResourceResolver resolver = null, 
			ElementTheme requestedTheme = ElementTheme.Light, string yesText = null, string noText = null);

        Task<bool> PromptAsync(string title, string content, MessageBoxType type, MessageBoxResult expected, IDialogResourceResolver resolver = null, 
			ElementTheme requestedTheme = ElementTheme.Light, string yesText = null, string noText = null);
    }
}
