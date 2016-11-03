using System;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.Navigation
{
    public class SuspensionLogic : ISuspensionLogic
    {
        public static ISuspensionLogic Instance { get; set; } = new SuspensionLogic();
        private SuspensionLogic()
        {
            // private constructor
        }

        public virtual async Task CallResumeAsync(ISuspensionAware viewmodel)
        {
            this.DebugWriteInfo();

            if (viewmodel != null)
            {
                var suspensionState = GetSuspensionState(viewmodel);
                await viewmodel.OnResumingAsync(suspensionState);
            }
        }

        public virtual async Task CallSuspendAsync(ISuspensionAware viewmodel)
        {
            this.DebugWriteInfo();

            if (viewmodel != null)
            {
                var suspensionState = GetSuspensionState(viewmodel);
                await viewmodel.OnResumingAsync(suspensionState);
                TimeStamp(suspensionState);
            }
        }

        #region private methods

        readonly string CacheDateKey = $"{nameof(SuspensionLogic)}.cache.date";

        protected INavigationParameters GetSuspensionState(ISuspensionAware viewmodel)
        {
            var rootContainer = ApplicationData.Current.LocalSettings;
            var key = $"{viewmodel.GetType()}";
            var values = rootContainer.CreateContainer(key, ApplicationDataCreateDisposition.Existing).Values;
            if (values.ContainsKey(key))
            {
                DateTime age;
                if (DateTime.TryParse(values[key]?.ToString(), out age))
                {
                    var setting = App.Settings.SuspensionStateExpires;
                    var expires = DateTime.Now.Subtract(setting);
                    var expired = expires <= age;
                    if (expired)
                    {
                        values.Clear();
                        TimeStamp(values);
                    }
                    else
                    {
                        // happiness
                    }
                }
                else
                {
                    TimeStamp(values);
                }
            }
            else
            {
                TimeStamp(values);
            }
            return values as INavigationParameters;
        }

        private void TimeStamp(IPropertySet values)
        {
            values[CacheDateKey] = DateTime.Now;
        }

        #endregion
    }
}