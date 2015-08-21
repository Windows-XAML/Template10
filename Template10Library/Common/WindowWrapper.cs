using System;
using System.Collections.Generic;
using System.Linq;
using Template10.Services.NavigationService;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Template10.Common
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-WindowWrapper
    public class WindowWrapper
    {
        Grid _busyIndicator;

        public static WindowWrapper Default() { return ActiveWrappers.FirstOrDefault(); }
        public readonly static List<WindowWrapper> ActiveWrappers = new List<WindowWrapper>();
        public static WindowWrapper Current() { return ActiveWrappers.FirstOrDefault(x => x.Window == Window.Current) ?? Default(); }
        public static WindowWrapper Current(Window window) { return ActiveWrappers.FirstOrDefault(x => x.Window == window); }
        public static WindowWrapper Current(NavigationService nav) { return ActiveWrappers.First(x => x.NavigationServices.Contains(nav)); }

        public WindowWrapper(Window window)
        {
            if (ActiveWrappers.Any(x => x.Window == window))
                throw new Exception("Windows already has a wrapper; use Current(window) to fetch.");
            Window = window;
            ActiveWrappers.Add(this);
            Dispatcher = new DispatcherWrapper(window.Dispatcher);
            window.Closed += (s, e) => { ActiveWrappers.Remove(this); };

            _busyIndicator = new Grid
            {
                Background = new SolidColorBrush(Colors.Black) { Opacity = .5 },
            };
            _busyIndicator.Children.Add(new ProgressRing
            {
                Height = 100,
                Width = 100,
                Foreground = new SolidColorBrush(Colors.White),
                IsActive = true,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            });
        }

        public bool IsBusy
        {
            get { return (Window.Content as Panel)?.Children.Contains(_busyIndicator) ?? false; }
            set
            {
                if (value && !IsBusy)
                    (Window.Content as Panel).Children.Add(_busyIndicator);
                else if (!value && IsBusy)
                    (Window.Content as Panel).Children.Remove(_busyIndicator);
            }
        }

        public void Close() { Window.Close(); }
        public Window Window { get; private set; }
        public DispatcherWrapper Dispatcher { get; private set; }
        public List<NavigationService> NavigationServices { get; } = new List<NavigationService>();
    }
}
