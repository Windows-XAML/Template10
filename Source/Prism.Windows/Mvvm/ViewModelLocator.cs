using Prism.Windows.Utilities;
using System;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;

namespace Prism.Windows.Mvvm
{
    public class ViewModelLocator
    {
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
            Prism.Mvvm.ViewModelLocationProvider.AutoWireViewModelChanged(d, Bind);
        }
        private static void Bind(object view, object viewmodel)
        {
            (view as FrameworkElement).DataContext = viewmodel;
        }
    }
}
