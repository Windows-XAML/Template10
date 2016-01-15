using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Template10.Utils
{
    public static class XamlUtils
    {
        public static T Ancestor<T>(this Control control) where T : Control
        {
            var parent = control.Parent as Control;
            while (parent != null)
            {
                if (parent is T) return (T)parent;
                parent = parent.Parent as Control;
            }
            return null;
        }

        public static List<T> AllChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            var list = new List<T>();
            var count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T)
                    list.Add(child as T);
                list.AddRange(AllChildren<T>(child));
            }
            return list;
        }
    }
}
