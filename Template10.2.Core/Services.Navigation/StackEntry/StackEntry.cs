using System;
using System.Linq;
using System.Collections.Generic;
using Template10.Services.Dispatcher;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.Navigation
{

    public class StackEntry : IStackEntry
    {
        public StackEntry(PageStackEntry entry)
        {
            Page = entry.SourcePageType;
            Parameter = entry.Parameter;
            NavigationTransitionInfo = entry.NavigationTransitionInfo;
        }

        public NavigationTransitionInfo NavigationTransitionInfo { get; set; }
        public Type Page { get; set; }
        public object Parameter { get; set; }
    }

}