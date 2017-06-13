using System;
using System.Collections.Generic;
using Template10.Common;
using Template10.Portable.Navigation;
using Template10.Portable.State;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public interface IFrameFacade
    {
        event EventHandler<HandledEventArgs> BackRequested;
        event EventHandler<HandledEventArgs> ForwardRequested;
        event EventHandler<NavigatedEventArgs> Navigated;
        event EventHandler<NavigatingEventArgs> Navigating;

        bool CanGoBack { get; }
        bool CanGoForward { get; }
        string FrameId { get; set; }
        object Content { get; }

        IList<PageStackEntry> BackStack { get; }
        IList<PageStackEntry> ForwardStack { get; }
    }
}