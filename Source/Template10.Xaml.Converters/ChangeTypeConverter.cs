using System;
using System.Reflection;
using Windows.UI.Xaml.Data;

namespace Template10.Converters
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Converters
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
            {
                return Activator.CreateInstance(targetType);
            }

            if (targetType.IsInstanceOfType(value))
            {
                return value;
            }

            return System.Convert.ChangeType(value, targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Convert(value, targetType, parameter, language);
        }
    }
}
