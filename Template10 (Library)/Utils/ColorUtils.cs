using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Template10.Utils
{
    public static class ColorUtils
    {
        public static SolidColorBrush ToSolidColorBrush(this Color color) => new SolidColorBrush(color);

        // this has all been moved to private bacause I am unhappy with the API 
        // and don't want anyone using it (except the HamburgerMenu) until we fix it.

        static Color nearBlack = Colors.Black.Lighten(Accents.Plus20);

        static Color nearWhite = Colors.Black.Lighten(Accents.Plus80);

        internal enum Accents : long
        {
            Plus90 = 90,
            Plus80 = 80,
            Plus70 = 70,
            Plus60 = 60,
            Plus50 = 50,
            Plus40 = 40,
            Plus30 = 30,
            Plus20 = 20,
            Plus10 = 10,
        }

        internal static float Lerp(this float start, float end, float amount)
        {
            float difference = end - start;
            float adjusted = difference * amount;
            return start + adjusted;
        }

        internal static Color Darken(this Color color, Accents amount)
        {
            var value = (float)amount;
            value = value * .01f;
            return Lerp(color, nearBlack, value);
        }

        internal static Color Lighten(this Color color, Accents amount)
        {
            var value = (float)amount;
            value = value * .01f;
            return Lerp(color, nearWhite, value);
        }

        internal static Color Lerp(this Color color, Color target, float amount)
        {
            float sr = color.R,
                sg = color.G,
                sb = color.B;
            float er = target.R,
                eg = target.G,
                eb = target.B;
            byte r = (byte)sr.Lerp(er, amount),
                 g = (byte)sg.Lerp(eg, amount),
                 b = (byte)sb.Lerp(eb, amount);
            return Color.FromArgb(255, r, g, b);
        }

    }
}
