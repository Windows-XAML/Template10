using System;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.Navigation
{

    public class NavigatingArgs : INavigatingArgs
    {
        public NavigatingArgs(NavigatingCancelEventArgs e)
        {
            NavigationMode = e.NavigationMode.ToNavigationModes();
            NavigationTransitionInfo = e.NavigationTransitionInfo;
            Parameter = e.Parameter;
            SourcePageType = e.SourcePageType;
        }

        public NavigationModes NavigationMode { get; set; }
        public object NavigationTransitionInfo { get; set; }
        public object Parameter { get; set; }
        public Type SourcePageType { get; set; }
    }

}