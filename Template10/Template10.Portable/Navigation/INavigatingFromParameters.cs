using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Core;

namespace Template10.Navigation
{
    public interface INavigatingFromParameters : INavigationParameters
    {
        bool Suspending { get; }
        object Parameter { get; }
        IPropertyBagEx PageState { get; }
    }

    public class NavigatingFromParameters : NavigationParametersBase, INavigatingFromParameters
    {
        public NavigatingFromParameters(bool suspending, INavigationInfo from, INavigationInfo to, ISessionState sessionState)
            : base(from, to, sessionState)
        {
            Suspending = suspending;
        }

        public bool Suspending { get; }
        public object Parameter => FromNavigationInfo.Parameter;
        public IPropertyBagEx PageState => FromNavigationInfo.PageState;
    }
}