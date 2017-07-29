using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Portable;
using Template10.Portable.Common;

namespace Template10.Navigation
{
    public interface INavigatingToParameters : INavigationParameters
    {
        bool Resuming { get; }
    }

    public class NavigatingToParameters : NavigationParametersBase, INavigatingToParameters
    {
        public NavigatingToParameters(NavMode mode, INavigationInfo from, INavigationInfo to, bool resuming, IDictionary<string, object> sessionState)
            : base(from, to, sessionState)
        {
            this.Resuming = resuming;
            this.NavigationMode = mode;
        }
        public bool Resuming { get; }
        public NavMode NavigationMode { get; }
    }
}