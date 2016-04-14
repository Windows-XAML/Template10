using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Template10.Utils
{
    public static class ColorUtils
    {
        public static SolidColorBrush ToSolidColorBrush(this Color color) => new SolidColorBrush(color);

        internal enum Add : long
        {
            _90p = 90, _80p = 80, _70p = 70, _60p = 60, _50p = 50, _40p = 40, _30p = 30, _20p = 20, _10p = 10,
        }

        internal static Color Darken(this Color color, Add amount)
        {
            var value = ((int)amount) * -.01;
            return ChangeColorBrightness(color, (float)value);
        }

        internal static Color Lighten(this Color color, Add amount)
        {
            var value = ((int)amount) * .01;
            return ChangeColorBrightness(color, (float)value);
        }

        static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
        }
    }
}
