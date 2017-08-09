using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Template10.Navigation
{
    public interface IFrameEx : IFrameEx2
    {
        string FrameId { get; set; }
        object Content { get; }

        IList<PageStackEntry> BackStack { get; }
        IList<PageStackEntry> ForwardStack { get; }

        ElementTheme RequestedTheme { get; set; }
    }
}