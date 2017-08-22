using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Template10.Services.Dialog
{
    public interface IDialogService
    {
        Task<IUICommand> ShowAsync(string content, string title = default(string));
        Task<IUICommand> ShowAsync(string content, string title, params IUICommand[] commands);
    }
}
