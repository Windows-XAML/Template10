using System;
using System.Collections;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Template10.Converters
{
    /// <summary>
    ///     A converter that converts to <seealso cref="Visibility.Collapsed" /> when the value is either null or empty,
    ///     otherwise <seealso cref="Visibility.Visible" />. Applies to strings and any implementation of IEnumerable
    /// </summary>
    public class CollapsedWhenNullOrEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value == null)
                    return Visibility.Collapsed;
                var s = value as string;
                if (s != null)
                    return string.IsNullOrWhiteSpace(s) ? Visibility.Collapsed : Visibility.Visible;
                var enumerable = value as IEnumerable;
                if (enumerable != null)
                {
                    return enumerable.Cast<object>().Any() ? Visibility.Visible : Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}