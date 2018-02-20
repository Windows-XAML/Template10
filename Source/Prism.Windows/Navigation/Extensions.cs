using Prism.Ioc;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Navigation;

namespace Prism.Navigation
{
    public static partial class Extensions
    {
        public static string GetNavigationPath(this INavigationService service)
        {
            var nav = service as IPlatformNavigationService2;
            var facade = nav.FrameFacade as IFrameFacade2;
            var sb = new List<string>();
            foreach (var item in facade.Frame.BackStack)
            {
                if (PageRegistry.TryGetRegistration(item.SourcePageType, out var info))
                {
                    sb.Add(info.Key);
                }
            }
            return string.Join("/", sb.ToArray());
        }

        public static async Task<INavigationResult> NavigateAsync(this INavigationService service, string path, params (string Name, string Value)[] parameters)
        {
            var p = new NavigationParameters();
            foreach (var parameter in parameters)
            {
                p.Add(parameter.Name, parameter.Value);
            }
            return await service.NavigateAsync(path, p);
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
    }
}
