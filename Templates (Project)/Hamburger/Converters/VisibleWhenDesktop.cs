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
    class VisibleWhenDesktop : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (DeviceUtils.Current().DeviceDisposition() == DeviceUtils.DeviceDispositions.Desktop) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
