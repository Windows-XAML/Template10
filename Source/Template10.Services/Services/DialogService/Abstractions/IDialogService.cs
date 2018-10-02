using System.Threading.Tasks;

namespace Template10.Services.Dialog
{
    public interface IDialogService
    {
        Task<MessageBoxResult> AlertAsync(string content, IDialogResourceResolver resolver = null);

        Task<MessageBoxResult> AlertAsync(string title, string content, IDialogResourceResolver resolver = null);

        Task<MessageBoxResult> PromptAsync(string content, MessageBoxType type = MessageBoxType.YesNo, IDialogResourceResolver resolver = null);

        Task<MessageBoxResult> PromptAsync(string title, string content, MessageBoxType type = MessageBoxType.YesNo, IDialogResourceResolver resolver = null);

        Task<bool> PromptAsync(string content, MessageBoxType type, MessageBoxResult expected, IDialogResourceResolver resolver = null);

        Task<bool> PromptAsync(string title, string content, MessageBoxType type, MessageBoxResult expected, IDialogResourceResolver resolver = null);
    }
}
