using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Common
{
    public class WindowWrapper
    {
        readonly static List<WindowWrapper> viewWrappers = new List<WindowWrapper>();
        public static WindowWrapper Current() { return viewWrappers.First(x => x.Window.Dispatcher.HasThreadAccess); }
        public static WindowWrapper Current(Window window) { return viewWrappers.First(x => x.Window == window); }

        public WindowWrapper(Window window)
        {
            Window = window;
            viewWrappers.Add(this);
            window.Closed += (s, e) => { viewWrappers.Remove(this); };
        }

        public Window Window { get; private set; }
        public IEnumerable<FrameFacade> Frames { get { return NavigationServices.Select(x => x.Frame); } }
        public List<Services.NavigationService.NavigationService> NavigationServices { get; } = new List<Services.NavigationService.NavigationService>();

        public async void Dispatch(Action action)
        {
            if (Window.Dispatcher.HasThreadAccess) { action?.Invoke(); }
            else { await Window.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action?.Invoke()); }
        }

        public async Task DispatchAsync(Func<Task> func)
        {
            if (Window.Dispatcher.HasThreadAccess) { await func?.Invoke(); }
            else
            {
                var tcs = new TaskCompletionSource<object>();
                await Window.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    try { func(); tcs.TrySetResult(null); }
                    catch (Exception ex) { tcs.TrySetException(ex); }
                });
                await tcs.Task;
            }
        }
    }
}
