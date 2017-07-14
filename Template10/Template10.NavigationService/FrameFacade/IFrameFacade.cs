using System.Collections.Generic;
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