using System.Collections.Generic;
using Template10.Common;
using Template10.Core;

namespace Template10.Navigation
{
    public interface INavigatedToParameters : INavigationParameters
    {
        bool Resuming { get; }
        NavMode NavigationMode { get; }
    }

    public class NavigatedToParameters : NavigationParametersBase, INavigatedToParameters
    {
        public NavigatedToParameters(NavMode mode, INavigationInfo from, INavigationInfo to, bool resuming, ISessionState sessionState)
            : base(from, to, sessionState)
        {
            this.Resuming = resuming;
            this.NavigationMode = mode;
        }
        public bool Resuming { get; }
        public NavMode NavigationMode { get; }
    }
}