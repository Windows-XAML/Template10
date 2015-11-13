using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using System.Reflection;

namespace Template10.Converters
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-Converters
    public class ChangeTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (targetType.IsConstructedGenericType && targetType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }
                targetType = Nullable.GetUnderlyingType(targetType);
            }

            if (value == null && targetType.GetTypeInfo().IsValueType)
                return Activator.CreateInstance(targetType);

            if (targetType == typeof(object))
                return (object)value;

            return System.Convert.ChangeType(value, targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Convert(value, targetType, parameter, language);
        }
    }
}
