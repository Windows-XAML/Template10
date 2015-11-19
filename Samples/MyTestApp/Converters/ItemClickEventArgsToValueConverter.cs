using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace MyTestApp.Converters {
	class ItemClickEventArgsToValueConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, string language) {
			return (value as ItemClickEventArgs)?.ClickedItem;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language) {
			throw new NotImplementedException();
		}
	}
}
