using System;
using Template10.Portable.PersistedDictionary;

namespace Template10.Portable.Navigation
{
    public interface INavigationInfo
    {
        Type PageType { get; }
        object Parameter { get; }
        IPersistedDictionary PageState { get; }
    }

    public class NavigationInfo : INavigationInfo
    {
        public NavigationInfo(Type pageType, object parameter, IPersistedDictionary pageState)
        {
            PageType = pageType;
            Parameter = parameter;
            PageState = pageState;
        }
        public Type PageType { get; }
        public object Parameter { get; }
        public IPersistedDictionary PageState { get; }
    }
}