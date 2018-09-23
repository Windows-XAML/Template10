﻿using Prism.Ioc;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Prism.Navigation
{
    public static partial class Extensions
    {
        internal static Frame GetXamlFrame(this INavigationService service)
        {
            return ((service as IPlatformNavigationService2).FrameFacade as IFrameFacade2).Frame;
        }

        public static string GetNavigationPath(this INavigationService service, bool includeParameters)
        {
            var nav = service as IPlatformNavigationService2;
            var facade = nav.FrameFacade as IFrameFacade2;
            var sb = new List<string>();
            foreach (var item in facade.Frame.BackStack)
            {
                if (PageRegistry.TryGetRegistration(item.SourcePageType, out var info))
                {
                    if (item.Parameter != null)
                    {
                        if (includeParameters)
                        {
                            sb.Add($"{info.Key}?{item.Parameter}");
                        }
                        else
                        {
                            sb.Add(info.Key);
                        }
                    }
                    else
                    {
                        sb.Add(info.Key);
                    }
                }
            }
            sb.Add(facade.CurrentNavigationPath);
            return $"/{string.Join("/", sb.ToArray())}";
        }

        public static async Task<INavigationResult> NavigateAsync(this INavigationService service, string path, params (string Name, string Value)[] parameters)
        {
            return await service.NavigateAsync(PathBuilder.Create(path, parameters).ToString());
        }

        public static async Task<INavigationResult> NavigateAsync(this INavigationService service, string path, NavigationTransitionInfo infoOverride = null, params (string Name, string Value)[] parameters)
        {
            return await service.NavigateAsync(PathBuilder.Create(path, parameters).ToString(), null, infoOverride);
        }

        public static bool TryGetParameter<T>(this NavigationEventArgs args, string name, out T value)
        {
            try
            {
                var www = new WwwFormUrlDecoder(args.Parameter.ToString());
                var result = www.GetFirstValueByName(name);
                value = (T)Convert.ChangeType(result, typeof(T));
                return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }

        public static bool TryGetParameters<T>(this NavigationEventArgs args, string name, out IEnumerable<T> values)
        {
            try
            {
                var www = new WwwFormUrlDecoder(args.Parameter.ToString());
                values = www
                    .Where(x => x.Name == name)
                    .Select(x => (T)Convert.ChangeType(x.Value, typeof(T)));
                return true;
            }
            catch
            {
                values = default(IEnumerable<T>);
                return false;
            }
        }
    }
}
