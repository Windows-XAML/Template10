using System;
using System.Threading;
using System.Threading.Tasks;
using Template10.Extensions;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Template10.Common
{
    public sealed class ViewService : IViewService
    {
        public static void OnWindowCreated()
        {
            var view = CoreApplication.GetCurrentView();
            if (!view.IsMain && !view.IsHosted)
            {
                var control = ViewLifetimeControl.GetForCurrentView();

                //This one time it should be made manually, as after Consolidate event fires the inner reference number should become zero
                control.StartViewInUse();

                //This is necessary to not make control.StartViewInUse()/control.StopViewInUse() manually on each and every async call. Facade will do it for you
                SynchronizationContext.SetSynchronizationContext(new SecondaryViewSynchronizationContextDecorator(control,
                    SynchronizationContext.Current));
            }
        }

        public async Task<IViewLifetimeControl> OpenAsync(UIElement content, string title = null,
            ViewSizePreference size = ViewSizePreference.UseHalf)
        {
            this.Log($"Frame: {content}, Title: {title}, Size: {size}");

            var currentView = ApplicationView.GetForCurrentView();
            title = title ?? currentView.Title;

            var newView = CoreApplication.CreateNewView();
            var dispatcher = DispatcherEx.Create(newView.Dispatcher);
            var newControl = await dispatcher.Dispatch(async () =>
            {
                var control = ViewLifetimeControl.GetForCurrentView();
                var newWindow = Window.Current;
                var newAppView = ApplicationView.GetForCurrentView();
                newAppView.Title = title;

                // TODO: (Jerry)
                // control.NavigationService = nav;

                newWindow.Content = content;
                newWindow.Activate();

                await ApplicationViewSwitcher
                    .TryShowAsStandaloneAsync(newAppView.Id, ViewSizePreference.Default, currentView.Id, size);
                return control;
            }).ConfigureAwait(false);
            return newControl;
        }
    }
}
