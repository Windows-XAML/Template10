using System;
using System.Linq;
using Windows.Foundation;

namespace Template10.Utils
{
    public static class UriUtils
    {
        public static Uri GetRoot(this Uri uri)
        {
            if (string.IsNullOrEmpty(uri.Query)) return new Uri(uri.ToString(), UriKind.RelativeOrAbsolute);
            else return new Uri(uri.ToString().Split('?')[0], UriKind.RelativeOrAbsolute);
        }

        public static WwwFormUrlDecoder QueryString(this Uri uri) => new WwwFormUrlDecoder(uri.Query);

        public static Uri RemoveQueryStringItem(this Uri uri, string name)
        {
            if (string.IsNullOrEmpty(uri.Query))
            {
                return uri;
            }
            var strings = uri.QueryString().Where(x => !x.Name.Equals(name));
            if (!strings.Any())
            {
                return uri.GetRoot();
            }
            var querystring = string.Join("&", strings.Select(x => $"{x.Name}={Uri.EscapeDataString(x.Value)}"));
            return new Uri($"{uri.GetRoot()}?{querystring}", UriKind.RelativeOrAbsolute);
        }

        public static Uri AppendQueryStringItem(this Uri uri, string name, string value)
        {
            string querystring = $"{name}={Uri.EscapeDataString(value)}";
            if (!string.IsNullOrEmpty(uri.Query))
            {
                querystring += "&" + string.Join("&", uri.QueryString().Select(x => $"{x.Name}={Uri.EscapeDataString(x.Value)}"));
            }
            return new Uri($"{uri.GetRoot()}?{querystring}", UriKind.RelativeOrAbsolute);
        }
    }
}
