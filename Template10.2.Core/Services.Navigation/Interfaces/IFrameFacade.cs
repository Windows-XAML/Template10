using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Services.Dispatcher;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Services.Navigation
{
    public interface IFrameFacade
    {
        event EventHandler<INavigatedArgs> Navigated;

        event EventHandler<INavigatingArgs> Navigating;

        IDispatcherService Dispatcher { get; }

        IReadOnlyList<IStackEntry> BackStack { get; }

        IReadOnlyList<IStackEntry> ForwardStack { get; }

        void ClearBackStack();

        object Content { get; }
    }
}
