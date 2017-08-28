using System;
using Windows.UI;

namespace Template10.Services.ColorService
{
    public interface IColorService
    {
        Color GetContrast(Color color);
        float GetBrightness(Color color);
        Single GetHue(Color color);
        float GetSaturation(Color color);
    }
}
