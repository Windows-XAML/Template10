namespace Template10.Services.ColorService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Windows.UI;

    // ColorHelper is a set of color conversion utilities
    public static class ColorHelper
    {
        #region structures

        public struct CMYK
        {
            public CMYK(float cyan, float magenta, float yellow, float black)
            {
                System.Diagnostics.Contracts.Contract.Assert(cyan >= 0 && cyan <= 255, "cyan range");
                System.Diagnostics.Contracts.Contract.Assert(magenta >= 0 && magenta <= 255, "magenta range");
                System.Diagnostics.Contracts.Contract.Assert(yellow >= 0 && yellow <= 255, "yellow range");
                System.Diagnostics.Contracts.Contract.Assert(black >= 0 && black <= 255, "black range");

                Cyan = cyan;
                Magenta = magenta;
                Yellow = yellow;
                Black = black;
            }

            public float Cyan;
            public float Magenta;
            public float Yellow;
            public float Black;
        }

        public struct HSL
        {
            public HSL(float hue, float saturation, float lightness)
            {
                System.Diagnostics.Contracts.Contract.Assert(hue >= 0 && hue <= 360, "hue range");
                System.Diagnostics.Contracts.Contract.Assert(saturation >= 0 && saturation <= 255, "saturation range");
                System.Diagnostics.Contracts.Contract.Assert(lightness >= 0 && lightness <= 255, "lightness range");

                Hue = hue;
                Saturation = saturation;
                Lightness = lightness;
            }

            public float Hue;
            public float Saturation;
            public float Lightness;
        }

        public struct HSV
        {
            public HSV(float hue, float saturation, float value)
            {
                System.Diagnostics.Contracts.Contract.Assert(hue >= 0 && hue <= 255, "hue range");
                System.Diagnostics.Contracts.Contract.Assert(saturation >= 0 && saturation <= 255, "saturation range");
                System.Diagnostics.Contracts.Contract.Assert(value >= 0 && value <= 255, "lightness range");

                Hue = hue;
                Saturation = saturation;
                Value = value;
            }

            public float Hue;
            public float Saturation;
            public float Value;
        }

        public struct RGB
        {
            public RGB(float red, float green, float blue)
            {
                System.Diagnostics.Contracts.Contract.Assert(red >= 0 && red <= 255, "red range");
                System.Diagnostics.Contracts.Contract.Assert(green >= 0 && green <= 255, "greeen range");
                System.Diagnostics.Contracts.Contract.Assert(blue >= 0 && blue <= 255, "blue range");

                Red = red;
                Green = green;
                Blue = blue;
            }

            public float Red;
            public float Green;
            public float Blue;
        }

        public struct HEX
        {
            public HEX(string hex)
            {
                System.Diagnostics.Contracts.Contract.Assert(!string.IsNullOrWhiteSpace(hex), "hex value");
                System.Diagnostics.Contracts.Contract.Assert(IsValid(hex), "hex length");

                Value = hex.Replace("#", string.Empty).Trim();
            }

            public string Value;

            public static bool IsValid(string value)
            {
                var correctLength = 6;
                return value.Replace("#", string.Empty).Trim().Length == correctLength;
            }
        }

        #endregion

        #region conversions

        public static HSL RGB2HSL(RGB rgb)
        {
            float R = rgb.Red / 255f;
            float G = rgb.Green / 255f;
            float B = rgb.Blue / 255f;

            float min = FindMin(R, G, B);
            float max = FindMax(R, G, B);
            float delta = max - min;

            float H = 0f;
            float S = 0f;
            float L = ((max + min) / 2.0f);

            if (delta != 0)
            {
                if (L < 0.5f)
                    S = delta / (max + min);
                else
                    S = delta / (2.0f - max - min);

                float dr = (((max - R) / 6) + (delta / 2)) / delta;
                float dg = (((max - G) / 6) + (delta / 2)) / delta;
                float db = (((max - B) / 6) + (delta / 2)) / delta;

                if (R == max)
                {
                    H = db - dg;
                }
                else if (G == max)
                {
                    H = (1 / 3) - db;
                }
                else if (B == max)
                {
                    if (dg < dr)
                        H = (2 / 3) + (dg);
                    else
                        H = (float)240 / 360 + (dg - dr);
                };

                if (H < 0)
                    H += 1;
                if (H > 1)
                    H -= 1;
                H *= 360;
            }

            H = (float)Math.Round(H);
            S = (float)Math.Round(S * 100);
            L = (float)Math.Round(L * 100);

            return new HSL(H, S, L);
        }
        public static HSV RGB2HSV(RGB rgb)
        {
            float R = rgb.Red / 255;
            float G = rgb.Green / 255;
            float B = rgb.Blue / 255;

            float max = Math.Max(Math.Max(R, B), G);
            float min = Math.Min(Math.Min(R, B), G);
            float delta = max - min;
            float Sat = (delta == 0) ? 0 : (Sat = delta / max);

            HSL hsl = RGB2HSL(rgb);
            float H = hsl.Hue;
            float S = Sat;
            float V = max;

            return new HSV(H, S, V);
        }
        public static HEX RGB2HEX(RGB rgb)
        {
            string R = (rgb.Red < 16) ? "0" + ((long)rgb.Red).ToString("X") : ((long)rgb.Red).ToString("X");
            string G = (rgb.Green < 16) ? "0" + ((int)rgb.Green).ToString("X") : ((int)rgb.Green).ToString("X");
            string B = (rgb.Blue < 16) ? "0" + ((int)rgb.Blue).ToString("X") : ((int)rgb.Blue).ToString("X");

            return new HEX(R + G + B);
        }
        public static CMYK RGB2CMYK(RGB rgb)
        {
            var R = rgb.Red / 255f;
            var G = rgb.Green / 255f;
            var B = rgb.Blue / 255f;

            var black = 1f;
            if (1 - R < black)
                black = 1 - R;
            if (1 - G < black)
                black = 1 - G;
            if (1 - B < black)
                black = 1 - B;

            var C = (1 - R - black) / (1 - black);
            C = (float)Math.Round(C * 100);
            var M = (1 - G - black) / (1 - black);
            M = (float)Math.Round(M * 100);
            var Y = (1 - B - black) / (1 - black);
            Y = (float)Math.Round(Y * 100);
            var K = (float)Math.Round(black * 100);

            return new CMYK(C, M, Y, K);
        }

        public static RGB HSL2RGB(HSL hsl)
        {
            if (hsl.Saturation == 0)
            {
                return new RGB(hsl.Lightness, hsl.Lightness, hsl.Lightness);
            }
            else
            {
                double H = hsl.Hue / 360;
                double S = hsl.Saturation / 255;
                double L = hsl.Lightness / 255;

                double valueA;
                if (L < .5) { valueA = L * (1 + S); }
                else { valueA = (L + S) - (S * L); }
                double valueB = 2 * L - valueA;

                Func<double, double, double, float> hueToRGB = (valA, valB, Hue) =>
                {
                    if (Hue < 0)
                        Hue += 1;
                    if (Hue > 1)
                        Hue -= 1;
                    if (6 * Hue < 1)
                        return (float)(valA + (valB - valA) * 6 * Hue);
                    if (2 * Hue < 1)
                        return (float)valB;
                    if (3 * Hue < 2)
                        return (float)(valA + (valB - valA) * ((2 / 3) - Hue) * 6);
                    return (float)valA;
                };

                float R = 255f * hueToRGB(valueB, valueA, H + 1 / 3);
                float G = 255f * hueToRGB(valueB, valueA, H);
                float B = 255f * hueToRGB(valueB, valueA, H - 1 / 3);
                return new RGB(R, G, B);
            }
        }
        public static RGB HSV2RGB(HSV hsv) { throw new NotImplementedException(); }
        public static RGB HEX2RGB(HEX hex)
        {
            string r = (hex.Value[0].ToString() + hex.Value[1].ToString());
            string g = (hex.Value[2].ToString() + hex.Value[3].ToString());
            string b = (hex.Value[4].ToString() + hex.Value[5].ToString());

            int R = int.Parse(r, System.Globalization.NumberStyles.HexNumber);
            int G = int.Parse(g, System.Globalization.NumberStyles.HexNumber);
            int B = int.Parse(b, System.Globalization.NumberStyles.HexNumber);

            return new RGB(R, G, B);
        }
        public static RGB CMYK2RGB(CMYK cmyk)
        {
            float R = (float)Math.Round(255 * (1 - (cmyk.Cyan / 100)) * (1 - (cmyk.Black / 100)));
            float G = (float)Math.Round(255 * (1 - cmyk.Magenta / 100) * (1 - cmyk.Black / 100));
            float B = (float)Math.Round(255 * (1 - cmyk.Yellow / 100) * (1 - cmyk.Black / 100));
            return new RGB(R, G, B);
        }

        #endregion

        #region varietals

        // http://en.wikipedia.org/wiki/Color_scheme#Complementary_colors
        public static RGB GetComplimentary(RGB rgb)
        {
            float R = Math.Abs(rgb.Red - 255);
            float G = Math.Abs(rgb.Green - 255);
            float B = Math.Abs(rgb.Blue - 255);
            return new RGB(R, G, B);
        }

        // http://en.wikipedia.org/wiki/Chromatics_(graphics)
        public static IEnumerable<RGB> GetChromatics(RGB rgb, int count = 10)
        {
            HSV hsv = RGB2HSV(rgb);
            RGB[] result = new RGB[count];
            for (int i = 1; i <= count; i++)
            {
                float H = hsv.Hue;
                float S = hsv.Saturation;
                float V = 255 * (float)(1 / i);
                HSV temp = new HSV(H, S, V);
                result[i] = HSV2RGB(temp);
            }
            return result;
        }

        // http://en.wikipedia.org/wiki/Color_scheme#Triadic_colors
        public static IEnumerable<RGB> GetTriads(RGB rgb)
        {
            RGB left = new RGB(rgb.Green, rgb.Blue, rgb.Red);
            RGB middle = new RGB(rgb.Red, rgb.Green, rgb.Blue);
            RGB right = new RGB(rgb.Blue, rgb.Red, rgb.Green);
            return new RGB[3] { left, middle, right };
        }

        // http://en.wikipedia.org/wiki/Color_scheme#Tetradic_colors
        public static IEnumerable<RGB> GetTetradics(RGB rgb)
        {
            return new RGB[4]
            {
                new RGB(rgb.Red, rgb.Blue, rgb.Green),
                new RGB(rgb.Red, rgb.Green, rgb.Blue),
                new RGB(rgb.Blue, rgb.Red, rgb.Green),
                new RGB(rgb.Blue, rgb.Green, rgb.Red)
            };
        }

        // http://en.wikipedia.org/wiki/Weber%E2%80%93Fechner_law#The_case_of_vision
        public static RGB GetContrast(RGB rgb)
        {
            var hsl = RGB2HSL(rgb);
            hsl = new HSL((hsl.Hue + 180) % 360, hsl.Saturation, hsl.Lightness);
            return HSL2RGB(hsl);
        }

        // http://en.wikipedia.org/wiki/Tints_and_shades
        public static IEnumerable<RGB> GetShades(RGB rgb, int count = 10)
        {
            HSL hsl = RGB2HSL(rgb);
            RGB[] result = new RGB[count];
            for (int i = 1; i <= count - 1; i++)
            {
                var H = hsl.Hue;
                var S = hsl.Saturation;
                var L = 255 * (float)(1 / i);
                HSL temp = new HSL(H, S, L);
                result[i] = HSL2RGB(temp);
            }
            return result;
        }

        #endregion

        // utils

        private static float FindMin(params float[] values) { return values.Min(); }
        private static float FindMax(params float[] values) { return values.Max(); }
    }

    // ColorExtensions implements ColorHelper for Windows.UI.Color
    // for example, you can use Colors.Red.GetComplimentary()
    public static class ColorExtensions
    {
        #region conversions

        public static ColorHelper.RGB ToRGB(this Color color) { return new ColorHelper.RGB(color.R, color.G, color.B); }
        public static ColorHelper.HSL ToHSL(this Color color) { return ColorHelper.RGB2HSL(color.ToRGB()); }
        public static ColorHelper.HSV ToHSV(this Color color) { return ColorHelper.RGB2HSV(color.ToRGB()); }
        public static ColorHelper.CMYK ToCMYK(this Color color) { return ColorHelper.RGB2CMYK(color.ToRGB()); }
        private static Color ToColor(this ColorHelper.RGB rgb) { return Color.FromArgb(255, (byte)rgb.Red, (byte)rgb.Green, (byte)rgb.Blue); }

        #endregion

        #region varietals

        public static Color GetComplimentary(this Color color) { return ColorHelper.GetComplimentary(color.ToRGB()).ToColor(); }
        public static IEnumerable<Color> GetChromatics(this Color color, int count = 10) { return ColorHelper.GetChromatics(color.ToRGB(), count).Select(x => x.ToColor()); }
        public static IEnumerable<Color> GetTriads(this Color color) { return ColorHelper.GetTriads(color.ToRGB()).Select(x => x.ToColor()); }
        public static IEnumerable<Color> GetTetradics(this Color color) { return ColorHelper.GetTetradics(color.ToRGB()).Select(x => x.ToColor()); }
        public static Color GetContrast(this Color color) { return ColorHelper.GetContrast(color.ToRGB()).ToColor(); }
        public static IEnumerable<Color> GetShades(this Color color, int count = 10) { return ColorHelper.GetShades(color.ToRGB()).Select(x => x.ToColor()); }

        #endregion
    }
}
