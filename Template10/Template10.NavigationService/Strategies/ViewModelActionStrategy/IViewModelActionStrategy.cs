using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Navigation;
using Template10.Mvvm;
using Template10.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Strategies
{
    public interface IViewModelActionStrategy
    {
        Task<bool> NavigatingToAsync((object ViewModel, NavigationMode NavigationMode, bool Resuming) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
        Task NavigatedToAsync((object ViewModel, NavigationMode NavigationMode, bool Resuming) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
        Task<bool> NavigatingFromAsync((object ViewModel, NavigationMode NavigationMode, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
        Task NavigatedFromAsync((object ViewModel, NavigationMode NavigationMode, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
    }
}