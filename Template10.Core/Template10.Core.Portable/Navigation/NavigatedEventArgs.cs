using System;

namespace Template10.Portable.Navigation
{
    public class NavigatedEventArgs : EventArgs
    {
        public NavigationMode NavigationMode { get; set; }
        public Type PageType { get; set; }
        public NavigationParameters Parameter { get; set; }
        public object Page { get; set; }
    }
}
