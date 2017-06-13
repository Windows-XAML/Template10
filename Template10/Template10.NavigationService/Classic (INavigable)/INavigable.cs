using System.Collections.Generic;
using Template10.Common;

namespace Template10.Services.NavigationService
{
    public interface INavigable : INavigatedAwareAsync, INavigatingAwareAsync
    {
        IDictionary<string, object> SessionState { get; }
        INavigationService NavigationService { get; set; }
        IDispatcherWrapper Dispatcher { get; set; }
    }
}