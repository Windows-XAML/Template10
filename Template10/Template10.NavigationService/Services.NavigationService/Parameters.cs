using System;
using Template10.Portable.Navigation;
using Windows.Foundation.Collections;

namespace Template10.Services.NavigationService
{
	public class NavigatedToParameters : NavigationParameters, INavigationParameters
    {
        public IPropertySet PageState { get; internal set; }
        public NavigationMode NavigationMode { get; internal set; }
        public Type SourceType { get; internal set; }
        public object SourceParameter { get; internal set; }
        public Type TargetType { get; internal set; }
        public object TargetParameter { get; internal set; }
    }

    public class NavigatedFromParameters : NavigationParameters, INavigationParameters
    {
        public Windows.Foundation.Collections.IPropertySet PageState { get; set; }
        public bool Suspending { get; internal set; }
    }

    public class NavigatingToParameters : NavigationParameters, INavigationParameters { }

    public class NavigatingFromParameters : NavigationParameters, INavigationParameters
    {
        public NavigationMode NavigationMode { get; internal set; }
        public Type SourceType { get; internal set; }
        public object SourceParameter { get; internal set; }
        public Type TargetType { get; internal set; }
        public object TargetParameter { get; internal set; }
        public bool Suspending { get; internal set; }
    }

    public class ConfirmNavigationParameters : NavigationParameters, INavigationParameters
    {
        public NavigationMode NavigationMode { get; internal set; }
        public Type SourceType { get; internal set; }
        public object SourceParameter { get; internal set; }
        public Type TargetType { get; internal set; }
        public object TargetParameter { get; internal set; }
        public bool Suspending { get; internal set; }
    }
}
