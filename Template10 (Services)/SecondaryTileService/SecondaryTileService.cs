using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;

namespace Template10.Services.SecondaryTileService
{
    public class SecondaryTileService : ISecondaryTileService
    {
        SecondaryTileHelper _helper = new SecondaryTileHelper();

        public Task<bool> PinAsync(TileInfo info, string tileId, string arguments)
        {
            return _helper.PinAsync(info, tileId, arguments);
        }

        public bool Exists(string tileId)
        {
            return _helper.Exists(tileId);
        }

        public Task<bool> UnpinAsync(string tileId, Rect selection, Placement placement = Placement.Above)
        {
            return _helper.UnpinAsync(tileId, selection, placement);
        }

        public Task<bool> UnpinAsync(string tileId)
        {
            return _helper.UnpinAsync(tileId);
        }
    }
}
