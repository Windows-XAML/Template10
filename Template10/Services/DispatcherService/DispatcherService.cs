using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Core;

namespace Windows10.Services.DispatcherService
{
    public class DispatcherService 
    {
        public CoreDispatcher Dispatcher { get; private set; }

        public DispatcherService(CoreDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }

        public async void SafeAction(Action action)
        {
            if (Dispatcher.HasThreadAccess)
            {
                action();
            }
            else
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
            }
        }
    }
}
