using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Portable.Navigation;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Strategies
{
    public abstract class ViewModelActionStrategyBase : IViewModelActionStrategy
    {
        public IDictionary<string, object> SessionState { get; set; } = Template10.Common.SessionStateHelper.Current;
        public abstract Task<bool> NavigatingToAsync((object ViewModel, NavigationMode NavigationMode, bool Resuming) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
        public abstract Task NavigatedToAsync((object ViewModel, NavigationMode NavigationMode, bool Resuming) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
        public abstract Task<bool> NavigatingFromAsync((object ViewModel, NavigationMode NavigationMode, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
        public abstract Task NavigatedFromAsync((object ViewModel, NavigationMode NavigationMode, bool Suspending) operation, INavigationInfo from, INavigationInfo to, INavigationService nav);
    }
}