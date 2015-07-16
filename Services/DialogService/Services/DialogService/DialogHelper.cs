using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Template10.Services.DialogService
{
    public class DialogHelper
    {
        bool _open = false;
        public DialogHelper()
        {
            // not thread safe
        }

        public async void Show(string content, string title = default(string))
        {
            await ShowAsync(content, title);
        }

        public async void Show(string content, string title = default(string), params UICommand[] commands)
        {
            await ShowAsync(content, title, commands);
        }

        public async Task ShowAsync(string content, string title = default(string), params UICommand[] commands)
        {
            while (_open)
            {
                await Task.Delay(1000);
            }
            _open = true;
            var dialog = new MessageDialog(content, title);
            if (commands != null && commands.Any())
                foreach (var item in commands)
                    dialog.Commands.Add(item);
            await dialog.ShowAsync();
            _open = false;
        }
    }
}
