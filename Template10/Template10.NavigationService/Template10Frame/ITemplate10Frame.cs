using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;

namespace Template10.Navigation
{
    public interface ITemplate10Frame
    {
        string FrameId { get; set; }
        object Content { get; }

        IList<PageStackEntry> BackStack { get; }
        IList<PageStackEntry> ForwardStack { get; }
    }
}