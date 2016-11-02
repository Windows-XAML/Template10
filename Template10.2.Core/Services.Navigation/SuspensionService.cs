using System;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.Navigation
{
    public class SuspensionService : ISuspensionService
    {
        public static SuspensionService Instance { get; } = new SuspensionService();
        private SuspensionService()
        {
            // private constructor
        }

        public async Task CallOnResumingAsync(String id, Page page, Int32 backStackDepth)
        {
            var vm = page?.DataContext as ISuspensionAware;
            if (vm != null)
            {
                var state = GetState(id, page.GetType(), backStackDepth);
                await vm.OnResumingAsync(state);
            }
        }

        public async Task CallOnSuspendingAsync(String id, Page page, Int32 backStackDepth)
        {
            var vm = page?.DataContext as ISuspensionAware;
            if (vm != null)
            {
                var state = GetState(id, page.GetType(), backStackDepth);
                await vm.OnSuspendingAsync(state);
            }
        }

        #region private methods

        readonly string CacheDateKey = $"{nameof(SuspensionService)}.cache.date";

        private IPropertySet GetState(string frameId, Type type, int backStackDepth)
        {
            var rootContainer = ApplicationData.Current.LocalSettings;
            var key = $"{frameId}-{type}-{backStackDepth}";
            var values = rootContainer.CreateContainer(key, ApplicationDataCreateDisposition.Existing).Values;
            if (values.ContainsKey(key))
            {
                DateTime age;
                if (DateTime.TryParse(values[key]?.ToString(), out age))
                {
                    var expiry = App.Settings.SuspensionStateExpires;
                    var expires = DateTime.Now.Subtract(expiry);
                    if (expires < age)
                    {
                        values.Clear();
                        Mark(values);
                    }
                    else
                    {
                        // happiness
                    }
                }
                else
                {
                    Mark(values);
                }
            }
            else
            {
                Mark(values);
            }
            return values;
        }

        private void Mark(IPropertySet values)
        {
            values[CacheDateKey] = DateTime.Now;
        }

        #endregion
    }
}