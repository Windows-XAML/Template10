using System.Collections.Generic;
using Template10.Portable.PersistedDictionary;

namespace Template10.Portable.Navigation
{
    public class NavigationParameters : INavigationParameters
    {
        public NavigationParameters(INavigationInfo from, INavigationInfo to, IDictionary<string, object> sessionState)
        {
            FromNavigationInfo = from;
            ToNavigationInfo = to;
            SessionState = SessionState;
        }

        public INavigationInfo FromNavigationInfo { get; }
        public INavigationInfo ToNavigationInfo { get; }
        public IDictionary<string, object> SessionState { get; }
    }
}
