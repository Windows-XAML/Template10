using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Template10.Portable.Navigation
{
    public class NavigationParameters : Dictionary<string, object>, INavigationParameters
    {
        public NavigationParameters()
        {
            // empty
        }

        public NavigationParameters(object parameter)
        {
            Parameter = parameter;
            TryQueryString();
        }

        private void TryQueryString()
        {
            if (Parameter != null && Parameter is string)
            {
                var pairs = Parameter
                    .ToString()
                    .Split(';')
                    .Where(x => x.Contains("="))
                    .Select(x => x.Split('='))
                    .Where(x => x.Count() == 2)
                    .Select(x => new
                    {
                        Key = Uri.UnescapeDataString(x[0]),
                        Value = Uri.UnescapeDataString(x[1])
                    });
                foreach (var pair in pairs)
                {
                    Add(pair.Key, pair.Value);
                }
            }
        }

        public object Parameter { get; set; }

        public bool TryGetParameter<T>(out T value)
        {
            try
            {
                value = (T)Parameter;
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
