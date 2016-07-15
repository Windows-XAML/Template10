using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Template10.Controls;

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

        public static void UpdateBindings(Page page)
        {
            var field = page.GetType().GetTypeInfo().GetDeclaredField("Bindings");
            var bindings = field?.GetValue(page);
            var update = bindings?.GetType().GetRuntimeMethod("Update", new Type[] { });
            update?.Invoke(bindings, null);
        }

        public static void InitializeBindings(Page page)
        {
            var field = page.GetType().GetTypeInfo().GetDeclaredField("Bindings");
            var bindings = field?.GetValue(page);
            var update = bindings?.GetType().GetRuntimeMethod("Initialize", new Type[] { });
            update?.Invoke(bindings, null);
        }

        public static void StopTrackingBindings(Page page)
        {
            var field = page.GetType().GetTypeInfo().GetDeclaredField("Bindings");
            var bindings = field?.GetValue(page);
            var update = bindings?.GetType().GetRuntimeMethod("StopTracking", new Type[] { });
            update?.Invoke(bindings, null);
        }

        public static T FirstAncestor<T>(this DependencyObject control) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(control) as DependencyObject;
            while (parent != null)
            {
                if (parent is T) return (T)parent;
                parent = VisualTreeHelper.GetParent(parent) as DependencyObject;
            }
            return null;
        }

        [Obsolete("Use FirstAncestor<T> instead", true)]
        public static T Ancestor<T>(this DependencyObject control) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(control) as DependencyObject;
            while (parent != null)
            {
                if (parent is T) return (T)parent;
                parent = VisualTreeHelper.GetParent(parent) as DependencyObject;
            }
            return null;
        }

        public static T FirstChild<T>(this DependencyObject parent) where T : DependencyObject => AllChildren<T>(parent).FirstOrDefault();

        public static List<DependencyObject> AllChildren(this DependencyObject parent)
        {
            var list = new List<DependencyObject>();
            var count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                list.AddRange(AllChildren(list.AddAndReturn(child)));
            }
            return list;
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

        public static void SetAsNotSet(this DependencyObject o, DependencyProperty dp)
        {
            o.SetValue(dp, DependencyProperty.UnsetValue);
        }

        public static void SetIfNotSet(this DependencyObject o, DependencyProperty dp, object value)
        {
            if (o.ReadLocalValue(dp) == DependencyProperty.UnsetValue)
                o.SetValue(dp, value);
        }

        /// <summary>
        /// Returns a list of submenu buttons with the same GroupName attribute as the command button upon which this
        /// extension is invoked (which is treated as Parent command button).
        /// </summary>
        /// <returns>List &lt; HamburgerButtonInfo &gt. List will be 0 if nothing found. </returns>
        /// <remarks>
        /// For added convenience, the GroupName attribute is detected with string.StartWith(groupName) rather than
        /// the straightforward string.Equals(groupName). That way we can tag submenu buttons as groupName1, groupName2, 
        /// groupName3, etc. With this scheme, the parent command button should be named by subset string, 
        /// which in this case is groupName.
        /// You don't have to use this scheme in which case you just stick to a single groupName for all buttons.
        /// </remarks>

        public static List<HamburgerButtonInfo> ItemsInGroup(this HamburgerButtonInfo button, bool IncludeSecondaryButtons = false)
        {
            string groupName = button.GroupName?.ToString();
            // Return 0 count List rather than null
            if (string.IsNullOrWhiteSpace(groupName)) return new List<HamburgerButtonInfo>();

            FrameworkElement fe = button.Content as FrameworkElement;
            HamburgerMenu hamMenu = fe.FirstAncestor<HamburgerMenu>();

            List<HamburgerButtonInfo> NavButtons = hamMenu.PrimaryButtons.ToList();
            if (IncludeSecondaryButtons) NavButtons.InsertRange(NavButtons.Count, hamMenu.SecondaryButtons.ToList());

            List<HamburgerButtonInfo> groupItems = NavButtons.Where(x => !x.Equals(button) && (x.GroupName?.ToString()?.StartsWith(groupName)??false)).ToList();

            return groupItems;
        }
    }
}
