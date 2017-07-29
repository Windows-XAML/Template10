using System.Collections.Generic;
using Template10.Portable;

namespace Template10.Navigation
{
    public interface INavigationParameters
    {
        INavigationInfo FromNavigationInfo { get; }
        INavigationInfo ToNavigationInfo { get; }
        IDictionary<string, object> SessionState { get; }
    }

    public abstract class NavigationParametersBase : INavigationParameters
    {
        public NavigationParametersBase(INavigationInfo from, INavigationInfo to, IDictionary<string, object> sessionState)
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