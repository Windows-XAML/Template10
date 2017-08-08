using System.Collections.Generic;
using Template10.Common;

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
        public NavigationParametersBase(INavigationInfo from, INavigationInfo to, IPropertyBagEx sessionState)
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