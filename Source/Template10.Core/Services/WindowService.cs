using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Template10.Core.Services
{
    static class WindowService
    {
        static readonly List<Window> _instances = new List<Window>();

        public static void Register(Window window)
        {
            if (!_instances.Contains(window))
            {
                _instances.Add(window);
                window.Closed += Window_Closed;
            }
        }

        private static void Window_Closed(object sender, Windows.UI.Core.CoreWindowEventArgs e)
        {
            UnRegister(sender as Window);
        }

        private static void UnRegister(Window window)
        {
            window.Closed -= Window_Closed;
            _instances.Remove(window);
        }

        public static Window[] AllWindows => _instances.ToArray();

        public static bool TryGetWindow(FrameworkElement element, out Window window)
        {
            foreach (var item in AllWindows)
            {
                if (item.Content is UIElement ui)
                {
                    if (ui == element)
                    {
                        window = item;
                        return true;
                    }
                    var children = ui.VisualChildren();
                    if (children.Contains(element))
                    {
                        window = item;
                        return true;
                    }
                }
            }
            window = null;
            return false;
        }
    }
}
