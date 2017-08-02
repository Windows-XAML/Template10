using System;
using Windows.UI.Xaml.Navigation;

namespace Template10.Extensions
{
    public static class PortableExtensions
    {
        public static Portable.Navigation.NavMode ToPortableNavigationMode(this NavigationMode mode)
        {
            switch (mode)
            {
                case NavigationMode.New: return Portable.Navigation.NavMode.New;
                case NavigationMode.Back: return Portable.Navigation.NavMode.Back;
                case NavigationMode.Forward: return Portable.Navigation.NavMode.Forward;
                case NavigationMode.Refresh: return Portable.Navigation.NavMode.Refresh;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}