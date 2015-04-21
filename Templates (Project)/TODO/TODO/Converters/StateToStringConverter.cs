using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Template10.Converters
{
    class StateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var state = (Models.States)value;
            switch (state)
            {
                case Models.States.NotStarted:
                    return "Not Started";
                case Models.States.InProcess:
                    return "In Process";
                default:
                    return "Done";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
