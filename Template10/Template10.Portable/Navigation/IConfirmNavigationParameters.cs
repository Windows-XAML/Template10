using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Core;

namespace Template10.Navigation
{
    public interface IConfirmNavigationParameters : INavigationParameters
    {
        bool GoingTo(Type type);
    }

    public class ConfirmNavigationParameters : NavigationParametersBase, IConfirmNavigationParameters
    {
        public ConfirmNavigationParameters(INavigationInfo from, INavigationInfo to, ISessionState sessionState) 
            : base(from, to, sessionState)
        {
            // empty
        }

        public bool GoingTo(Type type) => ToNavigationInfo?.PageType?.Equals(type) ?? false;
    }
}
