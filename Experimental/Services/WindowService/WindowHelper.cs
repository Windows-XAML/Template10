using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.WindowService
{
    public class WindowHelper
    {
        private Window Main { get; set; }
        private Window Window { get; set; }
        private ApplicationView View { get; set; }
        private CoreApplicationView CoreView { get; set; }
        public bool IsOpen { get { return CoreView != null; } }

        public async Task ShowAsync<T>(object param = null, ViewSizePreference size = ViewSizePreference.UseHalf) 
        {
            if (this.IsOpen)
            {
                await CoreView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                {
                    this.Close();
                });
            }
            else
            {
                this.Main = Window.Current;
            }

            this.CoreView = CoreApplication.CreateNewView();
            await CoreView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                this.Window = Window.Current;
                var frame = new Frame();
                frame.NavigationFailed += (s, e) => { System.Diagnostics.Debugger.Break(); };
                this.Window.Content = frame;
                frame.Navigate(typeof(T), param);
                this.View = ApplicationView.GetForCurrentView();
                this.View.Consolidated += Helper_Consolidated;
            });
            if (await ApplicationViewSwitcher.TryShowAsStandaloneAsync(this.View.Id, size))
            {
                await ApplicationViewSwitcher.SwitchAsync(this.View.Id);
            }
        }

        void Helper_Consolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
        {
            this.Close();
        }

        public void Close()
        {
            if (this.IsOpen)
            {
                if (CoreApplication.GetCurrentView().IsMain)
                    return;
                this.View.Consolidated -= Helper_Consolidated;
                try
                {
                    this.Window.Close();
                }
                finally
                {
                    this.View = null;
                    this.Window = null;
                    this.CoreView = null;
                }

                // reactivate main
                var view = CoreApplication.GetCurrentView();
                view.CoreWindow.Activate();
            }
        }
    }
}