using System;

namespace Template10.Mobile.Services.NavigationService
{
    public class NavigatedEventArgs : EventArgs
    {
        public NavigationMode NavigationMode { get; set; }
        public Type PageType { get; set; }
        public object Parameter { get; set; }
        public object Page { get; set; }
    }
}
