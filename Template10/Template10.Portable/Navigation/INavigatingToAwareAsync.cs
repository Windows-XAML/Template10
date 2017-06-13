using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Portable.State;

namespace Template10.Portable.Navigation
{
    public interface INavigatingToParameters : INavigationParameters
    {
    }

    public class NavigatingToParameters : NavigationParameters, INavigatingToParameters
    {
        public NavigatingToParameters(NavMode mode, INavigationInfo from, INavigationInfo to, IDictionary<string, object> sessionState)
            : base(from, to, sessionState)
        {
            this.Mode = mode;
        }
        public NavMode Mode { get; }
    }

    public interface INavigatingToAwareAsync
    {
        Task<bool> OnNavigatingToAsync(INavigationParameters args);
    }
}