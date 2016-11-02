using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Services.Navigation
{

    internal interface IFrameFacadeInternal : IFrameFacade
    {
        bool Navigate(Type page);

        bool Navigate(Type page, object parameter);

        bool Navigate(Type page, object parameter, NavigationTransitionInfo info);

        bool CanGoBack { get; }

        bool GoBack();

        void GoBack(NavigationTransitionInfo info);

        bool CanGoForward { get; }

        bool GoForward();
    }

}