using System;
using System.Threading;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Template10.Utils
{
    public static class DialogExtensions
    {
        /*
         * Safely show dialogs without overlapping them, which is not allowed in WinRT
         */

        private readonly static BooleanEx DialogState = new BooleanEx { Showing = false };

        public static async Task<IUICommand> ShowAsyncEx(this MessageDialog messageDialog, ITemplate10Dispatcher dispatcher = null)
            => await ShowAsync(async () => await messageDialog.ShowAsync(), dispatcher);

        public static async Task<ContentDialogResult> ShowAsyncEx(this ContentDialog contentDialog, ITemplate10Dispatcher dispatcher = null)
            => await ShowAsync(async () => await contentDialog.ShowAsync(), dispatcher);

        private static readonly object showLockPoint = new object();

        private static async Task<T> ShowAsync<T>(Func<Task<T>> show, ITemplate10Dispatcher dispatcher)
        {
            lock (DialogState)
            {
                while (DialogState.Showing)
                {
                    Monitor.Wait(DialogState);
                }
                DialogState.Showing = true;
            }

            // this next line creates a dependecy on the WindowService
            dispatcher = dispatcher ?? Template10Window.Current().Dispatcher;

            dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));

            var result = await dispatcher.DispatchAsync(async () => await show());
            lock (DialogState)
            {
                DialogState.Showing = false;
                Monitor.PulseAll(DialogState);
            }
            return result;
        }

        private class BooleanEx
        {
            public bool Showing { get; set; }
        }
    }
}
