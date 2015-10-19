using System;
using Windows.UI.Xaml.Data;

namespace Template10.Converters
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string format = (parameter as string) ?? Format;
            if (format == null)
                return value;
            return string.Format(format, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public string Format { get; set; }
    }
}
