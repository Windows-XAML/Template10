using System;
using System.Diagnostics;
using Windows.UI.Xaml.Data;

namespace Template10.Converters
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-Converters
    public class ValueWhenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (Debug)
                Debugger.Break();
            try
            {
                if (object.Equals(value,parameter ?? When))
                    return Value;
                return Otherwise;
            }
            catch
            {
                return Otherwise;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (Debug)
                Debugger.Break();
            if (OtherwiseValueBack == null)
                throw new InvalidOperationException("Cannot ConvertBack if no OtherwiseValueBack is set!");
            try
            {
                if (object.Equals(value, Value))
                    return When;
                return OtherwiseValueBack;
            }
            catch
            {
                return OtherwiseValueBack;
            }
        }

        public object Value { get; set; }
        public object Otherwise { get; set; }
        public object When { get; set; }
        public object OtherwiseValueBack{ get; set; }
        public bool Debug { get; set; }
    }
}
