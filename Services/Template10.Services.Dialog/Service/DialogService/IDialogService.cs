using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Template10.Services.Dialog
{
    public interface IDialogService
    {
        Task<MessageBoxResult> AlertAsync(string content, IResourceResolver resolver = null);
        Task<MessageBoxResult> PromptAsync(string content, MessageBoxType type = MessageBoxType.YesNo, IResourceResolver resolver = null);
        Task<bool> PromptAsync(string content, MessageBoxType type, MessageBoxResult expected, IResourceResolver resolver = null);
    }
}
