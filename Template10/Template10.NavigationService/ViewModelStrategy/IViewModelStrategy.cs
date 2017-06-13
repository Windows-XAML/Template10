using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Portable.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public interface IViewModelStrategy
    {
        Task<bool> NavigatingToAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
        Task NavigatedToAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
        Task<bool> NavigatingFromAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
        Task NavigatedFromAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
    }
}