using Windows.UI.Xaml.Navigation;

namespace Blank1.Services.NavigationService
{
    public class NavigatingEventArgs : NavigatedEventArgs
    {
        public NavigatingEventArgs() { }
        public NavigatingEventArgs(NavigatingCancelEventArgs e)
        {
            this.NavigationMode = e.NavigationMode;
            this.PageType = e.SourcePageType;
            this.Parameter = e.Parameter?.ToString();
        }
        public bool Cancel { get; set; } = false;
        public bool Suspending { get; set; } = false;
    }
}