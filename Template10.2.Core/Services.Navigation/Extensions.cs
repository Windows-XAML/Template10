using System;
using System.Linq;
using System.Collections.Generic;
using Windows.Foundation.Collections;

namespace Template10.Services.Navigation
{

    public static partial class Extensions
    {
        public static NavigationModes ToNavigationModes(this Windows.UI.Xaml.Navigation.NavigationMode mode)
        {
            switch (mode)
            {
                case Windows.UI.Xaml.Navigation.NavigationMode.Back: return NavigationModes.Back;
                case Windows.UI.Xaml.Navigation.NavigationMode.Forward: return NavigationModes.Forward;
                case Windows.UI.Xaml.Navigation.NavigationMode.Refresh: return NavigationModes.Refresh;
                default: return NavigationModes.New;
            }
        }

        public static IPropertySet ToPropertySet(this string parameter)
        {
            var propertySet = new PropertySet();
            propertySet.Add(nameof(parameter), parameter);
            return propertySet;
        }
    }

}