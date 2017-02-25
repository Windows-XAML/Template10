using Template10.Common;

namespace Template10.Services.NavigationService
{
    public interface INavigable : INavigatedAwareAsync, INavigatingAwareAsync
    {
        INavigationService NavigationService { get; set; }
        IDispatcherWrapper Dispatcher { get; set; }
        IStateItems SessionState { get; set; }
    }
}