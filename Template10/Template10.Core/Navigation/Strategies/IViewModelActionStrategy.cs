using System.Threading.Tasks;
using Template10.Navigation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Strategies
{
    public enum ContinueResult { Continue, Stop }

    public interface IViewModelActionStrategy
    {
        Task<bool> NavigatingToAsync(object ViewModel, NavigationMode NavigationMode, bool Resuming, INavigationInfo from, INavigationInfo to, INavigationService nav);
        Task NavigatedToAsync(object ViewModel, NavigationMode NavigationMode, bool Resuming, INavigationInfo from, INavigationInfo to, INavigationService nav);
        Task<ContinueResult> NavigatingFromAsync(object ViewModel, NavigationMode NavigationMode, bool Suspending, INavigationInfo from, INavigationInfo to, INavigationService nav);
        Task NavigatedFromAsync(object ViewModel, NavigationMode NavigationMode, bool Suspending, INavigationInfo from, INavigationInfo to, INavigationService nav);
    }
}