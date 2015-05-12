using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Blank2.Common
{
    public class MyRectangle: Grid
    {
        private static int count = 1;

        public MyRectangle()
        {
            this.Background = new SolidColorBrush(Colors.SteelBlue);
            this.Width = 100;
            this.Height = 100;
            this.Margin = new Windows.UI.Xaml.Thickness(10);
            Debug.WriteLine("Created " + count++);
        }
    }
}
