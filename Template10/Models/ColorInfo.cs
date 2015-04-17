using Newtonsoft.Json;
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
        [JsonIgnore]
        public SolidColorBrush Brush { get { return new SolidColorBrush(this.Color); } }
        [JsonIgnore]
        public SolidColorBrush ContrastForegroundBrush { get; set; } = new SolidColorBrush(Colors.Black);
        public float Hue { get; set; }
        public float Brightness { get; set; }
        public float Saturation { get; set; }

        public int ColSpan { get; set; } = 1;
        public int RowSpan { get; set; } = 1;

        public ColorInfo Clone()
        {
            return new ColorInfo
            {
                Name = this.Name,
                Color = this.Color,
                ContrastForegroundBrush = this.ContrastForegroundBrush,
                Hue = this.Hue,
                Brightness = this.Brightness,
                Saturation = this.Saturation,
            };
        }

        public override string ToString()
        {
            return this.Color.ToString();
        }
    }
}
