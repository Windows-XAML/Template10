using System.Linq;
using System.Threading.Tasks;
using Template10.Extensions;
using Windows.UI.Popups;

namespace Template10.Services.Dialog
{
    public class DialogService : IDialogService
    {
        public async Task<IUICommand> ShowAsync(string content, string title = default(string))
            => await new MessageDialog(content, title).ShowAsyncEx();

        public async Task<IUICommand> ShowAsync(string content, string title, params IUICommand[] commands)
        {
            var dialog = new MessageDialog(content, title);
            if (commands != null && commands.Any())
            {
                foreach (var item in commands)
                {
                    dialog.Commands.Add(item);
                }
            }
            return await dialog.ShowAsyncEx();
        }
    }
}
