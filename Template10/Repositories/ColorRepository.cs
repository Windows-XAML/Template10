using System;
using System.Collections.Generic;
using System.Linq;
using Template10.Common;
using Template10.Services.ColorService;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Template10.Repositories
{
    public class ColorRepository
    {
        public async Task<IEnumerable<Models.ColorInfo>> GetColorsAsync()
        {
            await Task.Delay(0);
            var colors = typeof(Colors)
                .GetRuntimeProperties()
                .Select(x => new Models.ColorInfo
                {
                    Name = x.Name,
                    Color = (Color)x.GetValue(null),
                });
            return colors
                .Where(x => !x.Name.Equals("Black"))
                .OrderBy(x => x.Color.ToHSL().Hue);
        }

        public async Task<Models.ColorInfo> GetColorAsync(string name)
        {
            var colors = await this.GetColorsAsync();
            return colors.FirstOrDefault(x => x.Name.Equals(name));
        }
    }
}
