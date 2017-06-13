using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Portable.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public abstract class ViewModelStrategyBase : IViewModelStrategy
    {
        public IDictionary<string, object> SessionState { get; set; } = sessionState;

        public abstract Task<bool> NavigatingToAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
        public abstract Task NavigatedToAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
        public abstract Task<bool> NavigatingFromAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
        public abstract Task NavigatedFromAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
    }
}