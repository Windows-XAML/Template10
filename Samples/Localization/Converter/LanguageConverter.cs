using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace LocalizationSample.Converter
{
    public class LanguageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (string.IsNullOrWhiteSpace(value as string))
                return string.Empty;
            
            return new CultureInfo(value as string).DisplayName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
