using System.Threading.Tasks;
using Template10.Services.Dialog;
using Windows.UI.Popups;

namespace Template10.Services.Dialog
{
    public class DialogService : IDialogService
    {
        DialogHelper _helper;

        public DialogService()
        {
            _helper = new DialogHelper();
        }

        public async Task ShowAsync(string content, string title = default(string))
        {
            await this._helper.ShowAsync(content, title);
        }

        public async Task ShowAsync(string content, string title, params UICommand[] commands)
        {
            await this._helper.ShowAsync(content, title, commands);
        }
    }
}
