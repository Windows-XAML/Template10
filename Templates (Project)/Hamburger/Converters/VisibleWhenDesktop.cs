using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Utils;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Sample.Converters
{
    public class VisibleWhenZeroConverter : IValueConverter
    {
        public object Convert(object v, Type t, object p, string l) =>
            Equals(0d, (double)v) ? Visibility.Visible : Visibility.Collapsed;

        public object ConvertBack(object v, Type t, object p, string l) => null;
    }
}
