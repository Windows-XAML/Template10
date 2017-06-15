using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Portable.Navigation;
using Template10.Portable.PersistedDictionary;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public interface IFrameFacade
    {
        string FrameId { get; set; }
        object Content { get; }

        IList<PageStackEntry> BackStack { get; }
        IList<PageStackEntry> ForwardStack { get; }
    }
}