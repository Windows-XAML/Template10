using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Template10.Utils
{
    public static class XamlUtils
    {
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

        [Obsolete("Use the Page.UpdateBindings() extension method.", false)]
        public static void UpdateBindings(Page page)
        {
            var field = page.GetType().GetTypeInfo().GetDeclaredField("Bindings");
            var bindings = field?.GetValue(page);
            var update = bindings?.GetType().GetRuntimeMethod("Update", new Type[] { });
            update?.Invoke(bindings, null);
        }

        [Obsolete("Use the Page.InitializeBindings() extension method.", false)]
        public static void InitializeBindings(Page page)
        {
            var field = page.GetType().GetTypeInfo().GetDeclaredField("Bindings");
            var bindings = field?.GetValue(page);
            var update = bindings?.GetType().GetRuntimeMethod("Initialize", new Type[] { });
            update?.Invoke(bindings, null);
        }

        [Obsolete("Use the Page.StopTrackingBindings() extension method.", false)]
        public static void StopTrackingBindings(Page page)
        {
            var field = page.GetType().GetTypeInfo().GetDeclaredField("Bindings");
            var bindings = field?.GetValue(page);
            var update = bindings?.GetType().GetRuntimeMethod("StopTracking", new Type[] { });
            update?.Invoke(bindings, null);
        }

        [Obsolete("Use the DependecyObject.AllChildren() extension method.", false)]
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
