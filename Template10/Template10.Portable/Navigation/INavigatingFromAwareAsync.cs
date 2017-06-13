using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Portable.State;

namespace Template10.Portable.Navigation
{
    public interface INavigatingFromParameters : INavigationParameters
    {
        bool Suspending { get; }
    }

    public class NavigatingFromParameters : NavigationParameters, INavigatingFromParameters
    {
        public NavigatingFromParameters(bool suspending, INavigationInfo from, INavigationInfo to, IDictionary<string, object> sessionState)
            : base(from, to, sessionState)
        {
            Suspending = suspending;
        }
        public bool Suspending { get; }
    }

    public interface INavigatingFromAwareAsync
    {
        Task OnNavigatingFromAsync(INavigatingFromParameters args);
    }
}