using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Template10.Models
{
    public class ColorInfo : Controls.MyGridView.IVariableGridItem
    {
        public string Name { get; set; }
        public Windows.UI.Color Color { get; set; }
        public SolidColorBrush Brush { get { return new SolidColorBrush(this.Color); } }
        public SolidColorBrush ContrastForegroundBrush { get; set; }
        public float Hue { get; internal set; }
        public float Brightness { get; internal set; }
        public float Saturation { get; internal set; }

        public int ColSpan { get; set; } = 1;
        public int RowSpan { get; set; } = 1;
    }
}
