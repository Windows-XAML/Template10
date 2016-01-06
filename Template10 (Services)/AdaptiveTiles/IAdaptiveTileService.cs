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
		void SetDefaultPrimaryTile();
        void UpdatePrimaryTile(Tile tile);

		void SetDefaultSecondaryTile(string tileId);
		void UpdateSecondaryTile(Tile tile, string tileId);
    }
}
