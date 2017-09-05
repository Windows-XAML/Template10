using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
