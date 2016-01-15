using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-NavigationService
    public class NavigatingEventArgs: NavigatedEventArgs
    {
        public NavigatingEventArgs() { }
        public NavigatingEventArgs(NavigatingCancelEventArgs e, Page page)
        {
            NavigationMode = e.NavigationMode;
            PageType = e.SourcePageType;
            Parameter = e.Parameter;
            Page = page;
        }
        public bool Cancel { get; set; } = false;
        public bool Suspending { get; set; } = false;
    }
}