using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Template10.Converters
{
    // DarkenColorConverter class:
    // Allows you to darken the color of an XAML color resource.
    // This is currently utilised in the App.xaml class
    // To facilitate a good-looking default titlebar.
    public class DarkenColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double percentage = 0.8; // Default 
            if (value is SolidColorBrush)
            {
                if (parameter != null)
                {
                    double.TryParse(parameter.ToString(), out percentage);
                }
                Color color = (value as SolidColorBrush).Color;
                return Color.FromArgb((byte)(color.A * percentage), (byte)(color.R * percentage), (byte)(color.G * percentage), (byte)(color.B * percentage));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
