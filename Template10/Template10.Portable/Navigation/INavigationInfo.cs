using System;
using Template10.Portable;
using Template10.Portable.Common;

namespace Template10.Portable.Navigation
{
    public interface INavigationInfo
    {
        Type PageType { get; }
        object Parameter { get; }
        IPropertyBagAsync PageState { get; }
    }

    public class NavigationInfo : INavigationInfo
    {
        public NavigationInfo(Type pageType, object parameter, IPropertyBagAsync pageState)
        {
            PageType = pageType;
            Parameter = parameter;
            PageState = pageState;
        }
        public Type PageType { get; }
        public object Parameter { get; }
        public IPropertyBagAsync PageState { get; }
    }
}