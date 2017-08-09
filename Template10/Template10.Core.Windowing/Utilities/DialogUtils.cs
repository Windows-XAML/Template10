using System;
using System.Threading;
using System.Threading.Tasks;
using Template10.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Template10.Extensions
{
    public static class DialogExtensions
    {
        /*
         * Safely show dialogs without overlapping them, which is not allowed in WinRT
         */

        public static async Task<IUICommand> ShowAsyncEx(this MessageDialog messageDialog, IDispatcherEx dispatcher = null)
            => await ShowAsync(async () => await messageDialog.ShowAsync(), dispatcher);

        public static async Task<ContentDialogResult> ShowAsyncEx(this ContentDialog contentDialog, IDispatcherEx dispatcher = null)
            => await ShowAsync(async () => await contentDialog.ShowAsync(), dispatcher);

        static SemaphoreSlim _showAsyncSemaphoreSlim = new SemaphoreSlim(1, 1);
        private static async Task<T> ShowAsync<T>(Func<Task<T>> show, IDispatcherEx dispatcher)
        {
            await _showAsyncSemaphoreSlim.WaitAsync();
            try
            {
                dispatcher = dispatcher ?? WindowEx.Current().Dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
                return await dispatcher.DispatchAsync(async () => await show());
            }
            finally
            {
                _showAsyncSemaphoreSlim.Release();
            }
        }
    }
}
