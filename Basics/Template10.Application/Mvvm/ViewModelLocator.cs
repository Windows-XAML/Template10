using Prism.Ioc;
using Prism.Windows.Navigation;
using Windows.UI.Xaml;

namespace Prism.Windows.Mvvm
{

    public static class ViewModelLocator
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
            var view = d as FrameworkElement;
            if (view != null)
            {
                var vmType = Container.PageRegistry.ViewModelFactory(d.GetType());
                var vmObject = Container.Resolve(vmType);
                view.DataContext = vmObject;
            }
        }
    }
}
