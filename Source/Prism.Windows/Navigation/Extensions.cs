using Prism.Ioc;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Navigation;

namespace Prism.Windows.Navigation
{
    public static partial class Extensions
    {
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
