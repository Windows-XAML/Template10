using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Windows.Foundation;

namespace Template10.Navigation
{
    public static class PageNavigationRegistry
    {
        static Dictionary<string, Type> _internal = new Dictionary<string, Type>();

        public static void Register(string pageKey, Type PageType)
        {
            _internal.Add(pageKey, PageType);
        }

        public static bool TryGetInfo(string pageKey, out (string Key, Type PageType) info)
        {
            if (_internal.ContainsKey(pageKey))
            {
                info = (pageKey, _internal[pageKey]);
                return true;
            }
            else
            {
                info = (null, null);
                return false;
            }
        }

        public static bool TryGetInfo(Type pageType, out (string Key, Type PageType) info)
        {
            if (_internal.Any(x => x.Value == pageType))
            {
                var item = _internal.FirstOrDefault(x => x.Value == pageType);
                info = (item.Key, pageType);
                return true;
            }
            else
            {
                info = (null, null);
                return false;
            }
        }
    }
}
