using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Template10.Extensions
{
    public static class XamlExtensions
    {
#pragma warning disable CS0618 // Type or member is obsolete

        public static void UpdateBindings(this Page page)
        {
            var field = page.GetType().GetTypeInfo().GetDeclaredField("Bindings");
            var bindings = field?.GetValue(page);
            var update = bindings?.GetType().GetRuntimeMethod("Update", new Type[] { });
            update?.Invoke(bindings, null);
        }

        public static void InitializeBindings(this Page page)
        {
            var field = page.GetType().GetTypeInfo().GetDeclaredField("Bindings");
            var bindings = field?.GetValue(page);
            var update = bindings?.GetType().GetRuntimeMethod("Initialize", new Type[] { });
            update?.Invoke(bindings, null);
        }

        public static void StopTrackingBindings(this Page page)
        {
            var field = page.GetType().GetTypeInfo().GetDeclaredField("Bindings");
            var bindings = field?.GetValue(page);
            var update = bindings?.GetType().GetRuntimeMethod("StopTracking", new Type[] { });
            update?.Invoke(bindings, null);
        }

#pragma warning restore CS0618 // Type or member is obsolete

        public static IList<DependencyObject> AllParents(this DependencyObject control)
        {
            var list = new List<DependencyObject>();
            var parent = VisualTreeHelper.GetParent(control) as DependencyObject;
            if (parent != null)
            {
                list.Add(parent);
                list.AddRange(AllParents(parent));
            }
            return list;
        }

        public static bool TryFindParent<T>(this DependencyObject control, out T value) where T : DependencyObject
        {
            try
            {
                var parents = AllParents(control);
                value = parents.OfType<T>().First();
                return true;
            }
            catch (Exception)
            {
                value = default(T);
                return false;
            }
        }

        public static bool TryFindParent<T>(this DependencyObject control, string name, out T value) where T : DependencyObject
        {
            try
            {
                var parents = AllParents(control);
                value = parents.OfType<FrameworkElement>().First(x => x.Name == name) as T;
                return true;
            }
            catch (Exception)
            {
                value = default(T);
                return false;
            }
        }

        public static bool TryFindChild<T>(this DependencyObject control, string name, out T value) where T : DependencyObject
        {
            try
            {
                var children = AllChildren(control);
                value = children.OfType<FrameworkElement>().First(x => x.Name == name) as T;
                return true;
            }
            catch (Exception)
            {
                value = default(T);
                return false;
            }
        }

        public static bool TryFindChild<T>(this DependencyObject control, out T value) where T : DependencyObject
        {
            try
            {
                var children = AllChildren(control);
                value = children.OfType<T>().First();
                return true;
            }
            catch (Exception)
            {
                value = default(T);
                return false;
            }
        }

        public static List<DependencyObject> AllChildren(this DependencyObject parent)
        {
            var list = new List<DependencyObject>();
            var count = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                list.AddRange(AllChildren(list.AddAndReturn(child)));
            }
            return list;
        }

        //public static ElementTheme ToElementTheme(this ApplicationTheme theme)
        //{
        //    switch (theme)
        //    {
        //        case ApplicationTheme.Light:
        //            return ElementTheme.Light;
        //        case ApplicationTheme.Dark:
        //        default:
        //            return ElementTheme.Dark;
        //    }
        //}

        //public static ApplicationTheme ToApplicationTheme(this ElementTheme theme)
        //{
        //    switch (theme)
        //    {
        //        case ElementTheme.Default:
        //            return ApplicationTheme.Dark;
        //        case ElementTheme.Light:
        //            return ApplicationTheme.Light;
        //        case ElementTheme.Dark:
        //        default:
        //            return ApplicationTheme.Dark;
        //    }
        //}

        public static void SetAsNotSet(this DependencyObject o, DependencyProperty dp)
        {
            o.SetValue(dp, DependencyProperty.UnsetValue);
        }

        public static void SetIfNotSet(this DependencyObject o, DependencyProperty dp, object value)
        {
            if (o.ReadLocalValue(dp) == DependencyProperty.UnsetValue)
            {
                o.SetValue(dp, value);
            }
        }

        public static T GetResource<T>(this string resourceName, T otherwise)
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
