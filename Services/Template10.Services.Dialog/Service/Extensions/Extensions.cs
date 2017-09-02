using System;
using System.Threading;
using System.Threading.Tasks;
using Template10.Services.Dialog;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Template10.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Queues the WinRT message dialog to show
        /// </summary>
        /// <param name="dialog">message dialog</param>
        /// <param name="timeout">queue/wait timeout</param>
        /// <param name="token">cancellation token</param>
        /// <returns>IUICommand</returns>
        public static async Task<IUICommand> QueueAsync(this MessageDialog dialog, TimeSpan? timeout = null, CancellationToken? token = null)
            => await DialogManager.OneAtATimeAsync(async () => await dialog.ShowAsync(), timeout, token);

        /// <summary>
        /// Queues the Template10 messagebox to show
        /// </summary>
        /// <param name="dialog">message dialog</param>
        /// <param name="timeout">queue/wait timeout</param>
        /// <param name="token">cancellation token</param>
        /// <returns>MessageBoxResult</returns>
        public static async Task<MessageBoxResult> QueueAsync(this MessageBoxEx dialog, TimeSpan? timeout = null, CancellationToken? token = null)
            => await DialogManager.OneAtATimeAsync(async () => await dialog.ShowAsync(), timeout, token);

        /// <summary>
        /// Queues the Template10 content dialog to show
        /// </summary>
        /// <param name="dialog">message dialog</param>
        /// <param name="timeout">queue/wait timeout</param>
        /// <param name="token">cancellation token</param>
        /// <returns>ContentDialogResult</returns>
        public static async Task<ContentDialogResult> QueueAsync(this ContentDialogEx dialog, TimeSpan? timeout = null, CancellationToken? token = null)
            => await DialogManager.OneAtATimeAsync(async () => await dialog.ShowAsync(), timeout, token);

        /// <summary>
        /// Queues the WinRT content dialog to show
        /// </summary>
        /// <param name="dialog">message dialog</param>
        /// <param name="timeout">queue/wait timeout</param>
        /// <param name="token">cancellation token</param>
        /// <returns>IUICommand</returns>
        public static async Task<ContentDialogResult> QueueAsync(this ContentDialog dialog, TimeSpan? timeout = null, CancellationToken? token = null)
        {
            if (dialog is ContentDialogEx cx)
            {
                cx.Showing = ShowingStates.Queued;
            }
            return await DialogManager
                .OneAtATimeAsync(async () => await dialog.ShowAsync(), timeout, token);
        }
    }
}
