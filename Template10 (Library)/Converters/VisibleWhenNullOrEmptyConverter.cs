using System;
using System.Collections;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Template10.Converters
{
    /// <summary>
    ///     A converter that converts to <seealso cref="Visibility.Visible" /> when the value is either null or empty,
    ///     otherwise <seealso cref="Visibility.Collapsed" />. Applies to strings and any implementation of IEnumerable
    /// </summary>
    public class VisibleWhenNullOrEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value == null)
                    return Visibility.Visible;
                var s = value as string;
                if (s != null)
                    return string.IsNullOrWhiteSpace(s) ? Visibility.Visible : Visibility.Collapsed;
                var enumerable = value as IEnumerable;
                if (enumerable != null)
                {
                    return enumerable.Cast<object>().Any() ? Visibility.Collapsed : Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}