using System;
using Template10.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Navigation
{
    public class NavigatingEventArgs : NavigatedEventArgs
    {
        DeferralExManager Manager;
        public DeferralEx GetDeferral() => Manager.GetDeferral();

        public NavigatingEventArgs(DeferralExManager manager) : base()
        {
            Manager = manager;
        }

        public NavigatingEventArgs(DeferralExManager manager, NavigatingCancelEventArgs e, Type targetPageType, object parameter, object targetPageParameter) : this(manager)
        {
            NavigationMode = e.NavigationMode;
            PageType = e.SourcePageType;
            Parameter = parameter;
            TargetPageType = targetPageType;
            TargetPageParameter = targetPageParameter;
        }

        public bool Cancel { get; set; } = false;
        public bool Suspending { get; set; } = false;
        public Type TargetPageType { get; set; }
        public object TargetPageParameter { get; set; }
    }
}