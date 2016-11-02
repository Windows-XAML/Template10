using System;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace Template10.Services.Navigation
{
    public class SuspensionState : ISuspensionState
    {
        readonly string CacheDateKey = $"{nameof(SuspensionState)}.cache.date";

        internal SuspensionState(string frameId, Type type, int backStackDepth)
        {
            var rootContainer = ApplicationData.Current.LocalSettings;
            var key = $"{frameId}-{type}-{backStackDepth}";
            Values = rootContainer.CreateContainer(key, ApplicationDataCreateDisposition.Existing).Values;
            if (Values.ContainsKey(key))
            {
                DateTime age;
                if (DateTime.TryParse(Values[key]?.ToString(), out age))
                {
                    var expiry = Template10.Settings.RestoreExpires;
                    var expires = DateTime.Now.Subtract(expiry);
                    if (expires < age)
                    {
                        Values.Clear();
                        Mark();
                    }
                    else
                    {
                        // happiness
                    }
                }
                else
                {
                    Mark();
                }
            }
            else
            {
                Mark();
            }
        }

        public ISuspensionState Mark()
        {
            Values[CacheDateKey] = DateTime.Now;
            return this;
        }

        public IPropertySet Values { get; }
    }
}