using System;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.Navigation
{

    public class NavigatedArgs : INavigatedArgs
    {
        public NavigatedArgs(NavigationEventArgs e)
        {
            Content = e.Content;
            NavigationMode = e.NavigationMode.ToNavigationModes();
            NavigationTransitionInfo = e.NavigationTransitionInfo;
            Parameter = e.Parameter;
            SourcePageType = e.SourcePageType;
            Uri = e.Uri;
        }

        public object Content { get; set; }
        public NavigationModes NavigationMode { get; set; }
        public object NavigationTransitionInfo { get; set; }
        public object Parameter { get; set; }
        public Type SourcePageType { get; set; }
        public Uri Uri { get; set; }
    }

}