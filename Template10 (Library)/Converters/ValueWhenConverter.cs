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
                if (object.Equals(value, When))
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
            try
            {
                if (object.Equals(value, Value))
                    return this.When;
                return this.BackOtherwise;
            }
            catch
            {
                return this.BackOtherwise;
            }
        }

        public object Value { get; set; }
        public object Otherwise { get; set; }
        public object When { get; set; }
        public object BackOtherwise{ get; set; }

    }
}
