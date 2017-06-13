using System.Collections.Generic;
using Template10.Portable.State;

namespace Template10.Portable.Navigation
{

    public interface INavigationParameters
    {
        INavigationInfo FromNavigationInfo { get; }
        INavigationInfo ToNavigationInfo { get; }
        IDictionary<string, object> SessionState { get; }
    }
}