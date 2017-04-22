using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Template10.Services.DialogService
{
    public interface IDialogService
    {
        Task ShowAsync(string content, string title = default(string));
        Task ShowAsync(string content, string title, params UICommand[] commands);
    }
}
