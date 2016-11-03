using System;
using System.Threading.Tasks;
using Template10.BCL;
using Template10.Services.Lifetime;
using Template10.Services.Serialization;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Services.Navigation
{
    public class ViewModelService : IViewModelService
    {
        public static ViewModelService Instance { get; } = new ViewModelService();
        private ViewModelService()
        {
            // private constructor
        }

        public virtual object ResolveViewModel(Page page)
        {
            this.DebugWriteInfo();

            return page?.DataContext;
        }

        public virtual object ResolveViewModel(Type page)
        {
            this.DebugWriteInfo();

            return null;
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

        public virtual async Task CallNavigatingAsync(INavigatingAware vm, string parameters, NavigationModes mode)
        {
            this.DebugWriteInfo();

            if (vm != null)
            {
                await vm.OnNavigatingToAsync(parameters, mode);
            }
        }

        #region private methods

        readonly string CacheDateKey = $"{nameof(SuspensionService)}.cache.date";

        protected IPropertySet GetSuspensionState(ISuspensionAware viewmodel)
        {
            var rootContainer = ApplicationData.Current.LocalSettings;
            var key = $"{viewmodel.GetType()}";
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
            return values;
        }

        private void TimeStamp(IPropertySet values)
        {
            values[CacheDateKey] = DateTime.Now;
        }

        #endregion
    }
}
