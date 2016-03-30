using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Template10.Converters
{
    public class DPWhenValueConverter : DependencyObject, IValueConverter
    {
        public object DependencyParameter
        {
            get { return GetValue(DependencyParameterProperty); }
            set { SetValue(DependencyParameterProperty, value); }
        }

        public static readonly DependencyProperty DependencyParameterProperty =
            DependencyProperty.Register("DependencyParameter", typeof(object), typeof(DPWhenValueConverter), new PropertyMetadata(null));

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (object.Equals(When , null) || object.Equals(value, When))
                    return DependencyParameter;
                return Otherwise;
            }
            catch
            {
                return Otherwise;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public object When { get; set; }
        public object Otherwise { get; set; }
    }
}
