using System;
using System.Reflection;
using Windows.UI.Xaml.Controls;

namespace Template10
{
    public static class BindingUtilities
    {
        public static void UpdateBindings(this Page page)
        {
            if (page == null)
            {
                return;
            }
            var field = page.GetType().GetTypeInfo().GetDeclaredField("Bindings");
            var bindings = field?.GetValue(page);
            var update = bindings?.GetType().GetRuntimeMethod("Update", new Type[] { });
            update?.Invoke(bindings, null);
        }

        public static void InitializeBindings(this Page page)
        {
            if (page == null)
            {
                return;
            }
            var field = page.GetType().GetTypeInfo().GetDeclaredField("Bindings");
            var bindings = field?.GetValue(page);
            var update = bindings?.GetType().GetRuntimeMethod("Initialize", new Type[] { });
            update?.Invoke(bindings, null);
        }

        public static void StopTrackingBindings(this Page page)
        {
            if (page == null)
            {
                return;
            }
            var field = page.GetType().GetTypeInfo().GetDeclaredField("Bindings");
            var bindings = field?.GetValue(page);
            var update = bindings?.GetType().GetRuntimeMethod("StopTracking", new Type[] { });
            update?.Invoke(bindings, null);
        }
    }
}
