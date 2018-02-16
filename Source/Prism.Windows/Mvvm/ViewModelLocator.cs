using Prism.Windows.Utilities;
using System;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;

namespace Prism.Windows.Mvvm
{
    public class ViewModelLocator : IViewModelLocator
    {
        #region AutowireViewModel

        public static bool GetAutowireViewModel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutowireViewModelProperty);
        }
        public static void SetAutowireViewModel(DependencyObject obj, bool value)
        {
            obj.SetValue(AutowireViewModelProperty, value);
        }
        public static readonly DependencyProperty AutowireViewModelProperty =
            DependencyProperty.RegisterAttached("AutowireViewModel", typeof(bool),
                typeof(ViewModelLocator), new PropertyMetadata(false, AutowireViewModelChanged));
        private static void AutowireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement view && view != null)
            {
                var ViewModel = Central.Locator.ViewModelFactory(d.GetType());
                view.DataContext = Central.Container.Resolve(ViewModel);
            }
        }

        #endregion

        internal ViewModelLocator()
        {
            ViewFactory = DefaultViewFactory;
            ViewModelFactory = DefaultViewModelFactory;
        }

        public Func<string, Type> ViewFactory { get; set; }

        public Func<Type, Type> ViewModelFactory { get; set; }

        public bool TryUseFactories(string key, out (string Key, Type View, Type ViewModel) info)
        {
            var view = ViewFactory(key);
            if (view == null)
            {
                info = (key, null, null);
                return false;
            }
            else
            {
                info = (key, view, ViewModelFactory(view));
                return true;
            }
        }

        private Type DefaultViewFactory(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            var assembly = ReflectionUtilities.GetCallingAssembly(GetType().GetTypeInfo().Assembly);
            var assembly_name = assembly.FullName.Split(",").First();
            var types = from prefix in new[] { string.Empty, "Views.", "Pages." }
                        from suffix in new[] { string.Empty, "Page", "View" }
                        select $"{assembly_name}.{prefix}{key}{suffix}, {assembly.FullName}";
            var type = types.Select(x => Type.GetType(x)).Where(x => x != null).FirstOrDefault();
            return type;
        }

        private Type DefaultViewModelFactory(Type page)
        {
            if (page == null)
            {
                return null;
            }

            var name = page.Name;
            var assembly = ReflectionUtilities.GetCallingAssembly(GetType().GetTypeInfo().Assembly);
            var assembly_name = assembly.FullName.Split(",").First();
            var types = from prefix in new[] { string.Empty, "ViewModels." }
                        from suffix in new[] { string.Empty, "ViewModel" }
                        from moniker in new[] { name, name.Replace("Page", string.Empty), name.Replace("View", string.Empty) }
                        select $"{assembly_name}.{prefix}{moniker}{suffix}, {assembly.FullName}";
            var type = types.Select(x => Type.GetType(x)).Where(x => x != null).FirstOrDefault();
            return type;
        }
    }
}
