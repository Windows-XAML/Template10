using System.Collections.Generic;
using Template10.Common;
using Template10.Core;
using Template10.Navigation;

namespace Template10.Core
{
    public interface ITemplate10ViewModel
    {
        IWindowEx Window { get; }
        IDispatcherEx Dispatcher { get; }
        ISessionState SessionState { get; }
        INavigationService NavigationService { get; set; }
    }
}

