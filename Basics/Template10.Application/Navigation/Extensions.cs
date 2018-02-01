using Prism.Navigation;
using System;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;

namespace Prism.Windows.Navigation
{
    public static partial class Extensions
    {
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
