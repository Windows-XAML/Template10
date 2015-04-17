using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Template10.Services.ColorService
{
    public class ColorService
    {
        #region Color Stuff

        // kudos Brian Suda
        // http://24ways.org/2010/calculating-color-contrast/

        public Color GetContrast(Color color)
        {
            var yiq = ((color.R * 299) + (color.G * 587) + (color.B * 114)) / 1000;
            return (yiq >= 128) ? Colors.Black : Colors.White;
        }

        // kudos to WPF
        // http://referencesource.microsoft.com/#System.Drawing/commonui/System/Drawing/Color.cs,23adaaa39209cc1f

        public float GetBrightness(Color color)
        {
            var r = color.R / 255.0f;
            var g = color.G / 255.0f;
            var b = color.B / 255.0f;

            float max, min;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            return (max + min) / 2;
        }

        public Single GetHue(Color color)
        {
            if (color.R == color.G && color.G == color.B)
                return 0; // 0 makes as good an UNDEFINED value as any

            var r = color.R / 255.0f;
            var g = color.G / 255.0f;
            var b = color.B / 255.0f;

            float max, min;
            float delta;
            float hue = 0.0f;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            delta = max - min;

            if (r == max)
            {
                hue = (g - b) / delta;
            }
            else if (g == max)
            {
                hue = 2 + (b - r) / delta;
            }
            else if (b == max)
            {
                hue = 4 + (r - g) / delta;
            }
            hue *= 60;

            if (hue < 0.0f)
            {
                hue += 360.0f;
            }
            return hue;
        }

        public float GetSaturation(Color color)
        {
            var r = color.R / 255.0f;
            var g = color.G / 255.0f;
            var b = color.B / 255.0f;

            float max, min;
            float l, s = 0;

            max = r; min = r;

            if (g > max) max = g;
            if (b > max) max = b;

            if (g < min) min = g;
            if (b < min) min = b;

            // if max == min, then there is no color and
            // the saturation is zero.
            //
            if (max != min)
            {
                l = (max + min) / 2;
                if (l <= .5)
                {
                    s = (max - min) / (max + min);
                }
                else
                {
                    s = (max - min) / (2 - max - min);
                }
            }
            return s;
        }

        #endregion
    }
}
