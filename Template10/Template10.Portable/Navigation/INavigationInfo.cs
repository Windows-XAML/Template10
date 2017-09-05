using System;
using Template10.Common;

namespace Template10.Navigation
{
    public interface INavigationInfo
    {
        Type PageType { get; }
        object Parameter { get; }
        IPropertyBagEx PageState { get; }
    }

    public class NavigationInfo : INavigationInfo
    {
        public NavigationInfo(Type pageType, object parameter, IPropertyBagEx pageState)
        {
            PageType = pageType;
            Parameter = parameter;
            PageState = pageState;
        }
        public Type PageType { get; }
        public object Parameter { get; }
        public IPropertyBagEx PageState { get; }
    }
}