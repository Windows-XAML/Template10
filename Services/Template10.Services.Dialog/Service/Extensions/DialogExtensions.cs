using System;
using System.Threading;
using System.Threading.Tasks;
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

        static SemaphoreSlim _showAsyncSemaphoreSlim = new SemaphoreSlim(1, 1);
        private static async Task<T> ShowAsync<T>(Func<Task<T>> show)
        {
            await _showAsyncSemaphoreSlim.WaitAsync();
            try
            {
                return await show();
            }
            finally
            {
                _showAsyncSemaphoreSlim.Release();
            }
        }
    }
}
