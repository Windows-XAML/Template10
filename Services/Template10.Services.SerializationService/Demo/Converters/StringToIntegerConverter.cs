using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace SerializationService.Demo.Converters
{
    public sealed class StringToIntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string str)
            {
                if (int.TryParse(str, out int result))
                {
                    return result;
                }
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is int)
            {
                return value.ToString();
            }
            return string.Empty;
        }
    }
}
