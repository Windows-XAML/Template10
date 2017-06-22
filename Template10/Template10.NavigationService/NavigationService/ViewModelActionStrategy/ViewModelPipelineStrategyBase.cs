using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Portable.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public abstract class ViewModelActionStrategyBase : IViewModelActionStrategy
    {
        public IDictionary<string, object> SessionState { get; set; } = Template10.Common.SessionState.Current;

        public abstract Task<bool> NavigatingToAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode, bool Resuming) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
        public abstract Task NavigatedToAsync((object ViewModel, Windows.UI.Xaml.Navigation.NavigationMode NavigationMode, bool Resuming) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
        public abstract Task<bool> NavigatingFromAsync((object ViewModel, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
        public abstract Task NavigatedFromAsync((object ViewModel, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
    }
}