using Windows.UI.Xaml;

namespace Template10.Utils
{
    public static class XamlResourceUtils
    {
        public static T GetResource<T>(string resourceName, T otherwise)
        {
            try
            {
                return (T)Application.Current.Resources[resourceName];
            }
            catch
            {
                return otherwise;
            }
        }
    }
}
