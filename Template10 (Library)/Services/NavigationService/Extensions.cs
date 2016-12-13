using System;
using Portable = Prism.Navigation;
using UWP = Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public static class Extensions
    {
        public static Portable.NavigationMode ToPrismNavigationMode(this UWP.NavigationMode mode)
        {
            switch (mode)
            {
                case UWP.NavigationMode.New: return Portable.NavigationMode.New;
                case UWP.NavigationMode.Back: return Portable.NavigationMode.Back;
                case UWP.NavigationMode.Forward: return Portable.NavigationMode.Forward;
                case UWP.NavigationMode.Refresh: return Portable.NavigationMode.Refresh;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}