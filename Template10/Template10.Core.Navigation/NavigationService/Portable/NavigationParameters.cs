using System;
using System.Collections.Generic;
using System.Linq;
using Template10.Services.StateService;

namespace Template10.Portable.Navigation
{
    public class NavigationParameters : INavigationParameters
    {
        public NavigationParameters(INavigationInfo from, INavigationInfo to, IStateContainer sessionState)
        {

        }

        public INavigationInfo FromNavigationInfo { get; }
        public INavigationInfo ToNavigationInfo { get; }
        public IStateContainer SessionState { get; }
    }
}
