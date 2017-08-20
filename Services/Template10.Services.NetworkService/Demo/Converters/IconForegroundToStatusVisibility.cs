using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Template10.Demo.NetworkService.Converters
{
    public class IconForegroundToStatusVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isOnColor = (value as SolidColorBrush)?.Color == Colors.Green;
            bool isOnLabel = parameter.ToString() == "On";

            if (isOnColor)
            {
                return (isOnLabel) ? Visibility.Visible : Visibility.Collapsed;
            }else
            {
                return (isOnLabel) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
