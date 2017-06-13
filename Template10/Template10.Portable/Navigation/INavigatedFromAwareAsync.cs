using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Portable.State;

namespace Template10.Portable.Navigation
{
    public interface INavigatedFromParameters : INavigationParameters
    {
        bool Suspending { get; }
    }

    public class NavigatedFromParameters : NavigationParameters, INavigatedFromParameters
    {
        public NavigatedFromParameters(bool suspending, NavMode mode, INavigationInfo from, INavigationInfo to, IDictionary<string, object> sessionState)
            : base(from, to, sessionState)
        {
            Suspending = suspending;
        }
        public bool Suspending { get; }
        public NavMode Mode { get; }
    }

    public interface INavigatedFromAwareAsync
    {
        Task OnNavigatedFromAsync(INavigatedFromParameters parameters);
    }
}