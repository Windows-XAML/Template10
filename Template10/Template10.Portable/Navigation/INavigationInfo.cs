using System;
using Template10.Portable.State;

namespace Template10.Portable.Navigation
{
    public interface INavigationInfo
    {
        Type PageType { get; }
        object Parameter { get; }
        IPersistedStateContainer PageState { get; }
    }

    public class NavigationInfo : INavigationInfo
    {
        public NavigationInfo(Type pageType, object parameter, IPersistedStateContainer pageState)
        {
            PageType = pageType;
            Parameter = parameter;
            PageState = pageState;
        }
        public Type PageType { get; }
        public object Parameter { get; }
        public IPersistedStateContainer PageState { get; }
    }
}