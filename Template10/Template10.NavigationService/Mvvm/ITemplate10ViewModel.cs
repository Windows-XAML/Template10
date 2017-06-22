using System.Collections.Generic;
using Template10.Common;
using Template10.Services.NavigationService;
using Template10.Services.WindowWrapper;

namespace Template10.Mvvm
{
    public interface ITemplate10ViewModel
    {
        IWindowWrapper Window { get; }
        IDispatcherWrapper Dispatcher { get; }
        IDictionary<string, object> SessionState { get; }
        INavigationService NavigationService { get; set; }
    }
}

