using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Portable.PersistedDictionary;

namespace Template10.Portable.Navigation
{
    public interface INavigatedToParameters : INavigationParameters
    {
    }

    public class NavigatedToParameters : NavigationParameters, INavigatedToParameters
    {
        public NavigatedToParameters(NavMode mode, INavigationInfo from, INavigationInfo to, IDictionary<string, object> sessionState)
            : base(from, to, sessionState)
        {
            this.Mode = mode;
        }
        public NavMode Mode { get; }
    }

    public interface INavigatedToAwareAsync
    {
        Task OnNavigatedToAsync(INavigatedToParameters parameter);
    }
}