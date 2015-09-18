using System;
using Windows.UI.Xaml.Data;

namespace Template10.Converters
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-Converters
    public class ValueWhenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value.Equals(When))
                    return this.Value;
                return this.Otherwise;
            }
            catch
            {
                return this.Otherwise;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public object Value { get; set; }
        public object Otherwise { get; set; }
        public object When { get; set; }
    }
}
