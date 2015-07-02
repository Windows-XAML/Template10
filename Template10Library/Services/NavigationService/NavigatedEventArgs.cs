using System;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public class NavigatedEventArgs : EventArgs
    {
        public NavigatedEventArgs() { }
        public NavigatedEventArgs(NavigationEventArgs e)
        {
            this.PageType = e.SourcePageType;
            this.Parameter = e.Parameter?.ToString();
            this.NavigationMode = e.NavigationMode;
        }
        public NavigationMode NavigationMode { get; set; }
        public Type PageType { get; set; }
        public string Parameter { get; set; }
    }
}
