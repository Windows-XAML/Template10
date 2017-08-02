using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Portable;
using Template10.Portable.Common;

namespace Template10.Mvvm
{
    public interface INavigatedToParameters : INavigationParameters
    {
        bool Resuming { get; }
        NavMode NavigationMode { get; }
    }

    public class NavigatedToParameters : NavigationParametersBase, INavigatedToParameters
    {
        public NavigatedToParameters(NavMode mode, INavigationInfo from, INavigationInfo to, bool resuming, IDictionary<string, object> sessionState)
            : base(from, to, sessionState)
        {
            this.Resuming = resuming;
            this.NavigationMode = mode;
        }
        public bool Resuming { get; }
        public NavMode NavigationMode { get; }
    }
}