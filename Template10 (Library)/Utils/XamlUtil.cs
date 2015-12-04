using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Template10.Utils
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-XamlHelper
    public static class XamlUtil
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> list) => new ObservableCollection<T>(list);

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

        public static ElementTheme ToElementTheme(this ApplicationTheme theme)
        {
            switch (theme)
            {
                case ApplicationTheme.Light:
                    return ElementTheme.Light;
                case ApplicationTheme.Dark:
                default:
                    return ElementTheme.Dark;
            }
        }

        public static ApplicationTheme ToApplicationTheme(this ElementTheme theme)
        {
            switch (theme)
            {
                case ElementTheme.Default:
                    return ApplicationTheme.Dark;
                case ElementTheme.Light:
                    return ApplicationTheme.Light;
                case ElementTheme.Dark:
                default:
                    return ApplicationTheme.Dark;
            }
        }

        public static T GetResource<T>(string resourceName, T otherwise)
        {
            try
            {
                return (T)Application.Current.Resources[resourceName];
            }
            catch
            {
                return otherwise;
            }
        }
    }
}
