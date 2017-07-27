using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Portable;
using Template10.Portable.Common;

namespace Template10.Mvvm
{
    public interface INavigatingFromParameters : INavigationParameters
    {
        bool Suspending { get; }
        object Parameter { get; }
        IPropertyBagAsync PageState { get; }
    }

    public class NavigatingFromParameters : NavigationParametersBase, INavigatingFromParameters
    {
        public NavigatingFromParameters(bool suspending, INavigationInfo from, INavigationInfo to, IDictionary<string, object> sessionState)
            : base(from, to, sessionState)
        {
            Suspending = suspending;
        }

        public bool Suspending { get; }
        public object Parameter => FromNavigationInfo.Parameter;
        public IPropertyBagAsync PageState => FromNavigationInfo.PageState;
    }
}