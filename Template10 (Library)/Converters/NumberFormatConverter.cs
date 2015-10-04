using System;
using Windows.UI.Xaml.Data;

namespace Template10.Converters
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-Converters
    public class NumberFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                // TODO: Is there a better way to do this?
                if (value is decimal) { return ((decimal)value).ToString(parameter.ToString()); }
                else if (value is double) { return ((double)value).ToString(parameter.ToString()); }
                else if (value is float) { return ((float)value).ToString(parameter.ToString()); }
                else if (value is int) { return ((int)value).ToString(parameter.ToString()); }
                else if (value is long) { return ((long)value).ToString(parameter.ToString()); }
                else if (value is short) { return ((short)value).ToString(parameter.ToString()); }
                else if (value is uint) { return ((uint)value).ToString(parameter.ToString()); }
                else if (value is ushort) { return ((ushort)value).ToString(parameter.ToString()); }
                else if (value is ulong) { return ((ulong)value).ToString(parameter.ToString()); }
                else
                {
                    return new ArgumentException("The Type of parameter 'value' is not supported.", "value");
                }
            }
            catch { return value; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
