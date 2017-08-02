using System.Collections.Generic;
using Template10.Common;
using Template10.Core;
using Template10.Navigation;

namespace Template10.Core
{
    public interface ITemplate10ViewModel
    {
        ITemplate10Window Window { get; }
        ITemplate10Dispatcher Dispatcher { get; }
        IDictionary<string, object> SessionState { get; }
        INavigationService NavigationService { get; set; }
    }
}

