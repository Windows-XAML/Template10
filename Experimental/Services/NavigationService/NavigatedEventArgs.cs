using System;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public class NavigatedEventArgs : EventArgs
    {
        private Windows.UI.Xaml.Navigation.NavigationEventArgs e;

        public NavigatedEventArgs() { }

        public NavigatedEventArgs(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            PageType = e.SourcePageType;
            Parameter = e.Parameter?.ToString();
            NavigationMode = e.NavigationMode;

        }

        public NavigationMode NavigationMode { get; set; }
        public Type PageType { get; set; }
        public string Parameter { get; set; }
    }
}
