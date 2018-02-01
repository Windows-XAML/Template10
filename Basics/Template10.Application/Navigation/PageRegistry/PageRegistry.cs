using Prism.Ioc;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Windows.Foundation;

namespace Prism.Windows.Navigation
{
    internal class PageRegistry : IPageRegistry
    {
        static Dictionary<string, (Type PageType, Type ViewModelType)> _pages = new Dictionary<string, (Type PageType, Type ViewModelType)>();

        public void Register(string pageKey, Type pageType, Type viewModelType)
        {
            _pages.Add(pageKey, (pageType, viewModelType));
        }

        public bool TryGetInfo(string pageKey, out (string Key, Type PageType, Type ViewModelType) info)
        {
            if (_pages.ContainsKey(pageKey))
            {
                info = (pageKey, _pages[pageKey].PageType, _pages[pageKey].ViewModelType);
                return true;
            }
            else
            {
                info = (null, null, null);
                return false;
            }
        }

        public bool TryGetInfo(Type pageType, out (string Key, Type PageType, Type ViewModelType) info)
        {
            if (_pages.Any(x => x.Value.PageType == pageType))
            {
                var item = _pages.FirstOrDefault(x => x.Value.PageType == pageType);
                info = (item.Key, item.Value.PageType, item.Value.ViewModelType);
                return true;
            }
            else
            {
                info = (null, null, null);
                return false;
            }
        }
    }
}
