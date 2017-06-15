using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Portable.PersistedDictionary;

namespace Template10.Portable.Navigation
{
    public interface IConfirmNavigationParameters : INavigationParameters
    {
        // empty
    }

    public class ConfirmNavigationParameters : NavigationParameters, IConfirmNavigationParameters
    {
        public ConfirmNavigationParameters(INavigationInfo from, INavigationInfo to, IDictionary<string, object> sessionState) : base(from, to, sessionState)
        {
            // empty
        }
    }

    public interface IConfirmNavigationAsync
    {
        Task<bool> CanNavigateAsync(IConfirmNavigationParameters parameters);
    }
}
