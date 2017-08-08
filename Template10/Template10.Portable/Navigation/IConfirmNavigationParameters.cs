using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Common;

namespace Template10.Navigation
{
    public interface IConfirmNavigationParameters : INavigationParameters
    {
        // empty
    }

    public class ConfirmNavigationParameters : NavigationParametersBase, IConfirmNavigationParameters
    {
        public ConfirmNavigationParameters(INavigationInfo from, INavigationInfo to, IPropertyBagEx sessionState) : base(from, to, sessionState)
        {
            // empty
        }
    }
}
