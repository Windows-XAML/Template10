using System;
using System.Threading;
using System.Threading.Tasks;
using Template10.Services.Dialog;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Template10.Extensions
{
    public static class DialogExtensions
    {
        /*
         * Safely show dialogs without overlapping them, which is not allowed in WinRT
         */

        public static async Task<IUICommand> ShowAsyncEx(this MessageDialog messageDialog)
            => await ShowAsync(async () => await messageDialog.ShowAsync());

        public static async Task<ContentDialogResult> ShowAsyncEx(this ContentDialog contentDialog)
            => await ShowAsync(async () => await contentDialog.ShowAsync());

        private static async Task<T> ShowAsync<T>(Func<Task<T>> show)
            => await DialogManager.OnlyOneAsync<T>(show);
    }
}
