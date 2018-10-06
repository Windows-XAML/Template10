using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Samples.Helpers
{
    public static class NewWindowHelper
    {
        public static async Task CreateWindowAsync(FrameworkElement content, string title)
        {
            var currentView = ApplicationView.GetForCurrentView();
            currentView.Title = title;

            var file = await BitmapImageHelper.SaveBitmapImageToFile(content, "render.jpg");
            var size = new Size(content.ActualWidth, content.ActualHeight);

            var newView = CoreApplication.CreateNewView();
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                var image = await BitmapImageHelper.ReadBitmapImageFromFile(size, file);
                var brush = new ImageBrush
                {
                    ImageSource = image,
                    Stretch = Stretch.None,
                    AlignmentX = AlignmentX.Center,
                    AlignmentY = AlignmentY.Center
                };

                var window = Window.Current;
                window.Content = new Grid { Background = brush };
                Window.Current.Activate();

                var preferences = ViewModePreferences.CreateDefault(ApplicationViewMode.CompactOverlay);
                preferences.ViewSizePreference = ViewSizePreference.Custom;
                preferences.CustomSize = new Size(size.Width * 1.25, size.Height * 1.25);

                var view = ApplicationView.GetForCurrentView();
                await ApplicationViewSwitcher.TryShowAsViewModeAsync(view.Id, ApplicationViewMode.CompactOverlay, preferences);
            });
        }
    }
}
