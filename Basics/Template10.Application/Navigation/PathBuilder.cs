using Prism.Navigation;
using System;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Template10.Navigation
{
    public class PathBuilder
    {
        private string _value;
        public PathBuilder(string value)
        {
            _value = value;
        }

        public static PathBuilder Create(bool clear, string page, params (string Name, string Value)[] parameters)
        {
            if (string.IsNullOrEmpty(page))
            {
                throw new ArgumentNullException(nameof(page));
            }
            var prefix = clear ? "/" : string.Empty;
            var value = Build(prefix, page, parameters);
            return new PathBuilder(value);
        }

        public PathBuilder Append(string page, params (string Name, string Value)[] parameters)
        {
            if (string.IsNullOrEmpty(page))
            {
                throw new ArgumentNullException(nameof(page));
            }
            _value += Build("/", page, parameters);
            return this;
        }

        private static string Build(string prefix, string page, (string Name, string Value)[] parameters)
        {
            var target = WebUtility.UrlEncode(page);
            var encoded = parameters?
                .Where(x => x.Name != null)
                .Where(x => x.Value != null)
                .Select(x => new
                {
                    Name = WebUtility.UrlEncode(x.Name),
                    Value = WebUtility.UrlEncode(x.Value),
                })
                .Select(x => $"{x.Name}={x.Value}");
            if (encoded.Any())
            {
                var querystring = string.Join('&', encoded);
                var value = $"{prefix}{page}?{querystring}";
                return value;
            }
            else
            {
                var value = $"/{page}";
                return value;
            }
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
