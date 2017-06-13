using System;
using UWP = Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public static class PortableExtensions
    {
        public static Template10.Portable.Navigation.NavMode ToPortableNavigationMode(this UWP.NavigationMode mode)
        {
            switch (mode)
            {
                case UWP.NavigationMode.New: return Template10.Portable.Navigation.NavMode.New;
                case UWP.NavigationMode.Back: return Template10.Portable.Navigation.NavMode.Back;
                case UWP.NavigationMode.Forward: return Template10.Portable.Navigation.NavMode.Forward;
                case UWP.NavigationMode.Refresh: return Template10.Portable.Navigation.NavMode.Refresh;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}