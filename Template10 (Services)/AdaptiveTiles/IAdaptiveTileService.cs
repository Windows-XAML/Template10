using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Services.AdaptiveTiles.Model;

namespace Template10.Services.AdaptiveTiles
{
    public interface IAdaptiveTileService
    {
        void UpdatePrimaryTile(Tile tile);
        void UpdateSecondaryTile(Tile tile, string tileId);
    }
}
