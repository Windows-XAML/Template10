using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Portable.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public interface IViewModelActionStrategy
    {
        Task<bool> NavigatingToAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode, bool Resuming) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
        Task NavigatedToAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode, bool Resuming) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
        Task<bool> NavigatingFromAsync((object ViewModel, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
        Task NavigatedFromAsync((object ViewModel, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
    }
}