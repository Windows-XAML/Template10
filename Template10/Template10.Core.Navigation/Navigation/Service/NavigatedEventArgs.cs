using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Navigation
{
    public class NavigatedEventArgs : EventArgs
    {
        public NavigatedEventArgs() { }
        public NavigatedEventArgs(NavigationEventArgs e, Page page)
        {
            PageType = e.SourcePageType;
            Parameter = e.Parameter;
            NavigationMode = e.NavigationMode;
        }

        public NavigationMode NavigationMode { get; set; }
        public Type PageType { get; set; }
        public object Parameter { get; set; }
    }
}
