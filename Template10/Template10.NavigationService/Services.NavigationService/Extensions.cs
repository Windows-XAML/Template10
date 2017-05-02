using System;
using UWP = Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
	public static class Extensions
    {
        public static Template10.Portable.Navigation.NavigationMode ToPortableNavigationMode(this UWP.NavigationMode mode)
        {
            switch (mode)
            {
                case UWP.NavigationMode.New: return Template10.Portable.Navigation.NavigationMode.New;
                case UWP.NavigationMode.Back: return Template10.Portable.Navigation.NavigationMode.Back;
                case UWP.NavigationMode.Forward: return Template10.Portable.Navigation.NavigationMode.Forward;
                case UWP.NavigationMode.Refresh: return Template10.Portable.Navigation.NavigationMode.Refresh;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}