using System;
using Template10.Utils;
using Windows.UI;
using Windows.UI.Xaml.Data;
using System.Text.RegularExpressions;

namespace Template10.Samples.BottomAppBarSample.Converters
{
    public class NumberColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Pattern to strip off currency symbol by retaining negative sign and decimal point.
            Regex regex = new Regex(@"[^\d-.]+");

            var d = regex.Replace(value.ToString(), string.Empty);
            return double.Parse(d) > 0 ? Colors.Green.ToSolidColorBrush() : Colors.Red.ToSolidColorBrush();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
