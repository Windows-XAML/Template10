using System.Collections.Generic;
using Template10.Common;
using Template10.Core;

namespace Template10.Navigation
{
    public interface INavigationParameters
    {
        INavigationInfo FromNavigationInfo { get; }
        INavigationInfo ToNavigationInfo { get; }
        IPropertyBagEx SessionState { get; }
    }

    public abstract class NavigationParametersBase : INavigationParameters
    {
        public NavigationParametersBase(INavigationInfo from, INavigationInfo to, ISessionState sessionState)
        {
            FromNavigationInfo = from;
            ToNavigationInfo = to;
            SessionState = SessionState;
        }

        public INavigationInfo FromNavigationInfo { get; }
        public INavigationInfo ToNavigationInfo { get; }
        public IPropertyBagEx SessionState { get; }
    }
}