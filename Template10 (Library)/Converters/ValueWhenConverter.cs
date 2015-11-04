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
                if (object.Equals(value,parameter ?? When))
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
            if (OtherwiseValueBack == null)
                throw new InvalidOperationException("Cannot ConvertBack if no OtherwiseValueBack is set!");
            try
            {
                if (object.Equals(value, Value))
                    return this.When;
                return this.OtherwiseValueBack;
            }
            catch
            {
                return this.OtherwiseValueBack;
            }
        }

        public object Value { get; set; }
        public object Otherwise { get; set; }
        public object When { get; set; }
        public object OtherwiseValueBack{ get; set; }
    }
}
