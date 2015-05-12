using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Blank3.Common
{
    class MySpecial: Grid
    {
        private static int count = 0;

        public MySpecial()
        {
            this.Height = this.Width = 100;
            this.Margin = new Windows.UI.Xaml.Thickness(10);
            this.Background = new SolidColorBrush(Colors.SteelBlue);
            Debug.WriteLine("Created " + count++);
        }
    }
}
