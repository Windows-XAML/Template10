using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Utils;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace BottomAppBar.Converters
{
    public class NumberColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var d = value.ToString().Replace("$", string.Empty).Replace("%", string.Empty).Replace(" ", string.Empty).Replace("(", "-").Replace(")", string.Empty);
            return double.Parse(d) > 0 ? Colors.Green.ToSolidColorBrush() : Colors.Red.ToSolidColorBrush();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
