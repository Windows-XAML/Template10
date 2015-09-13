﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Services.AdaptiveTiles.Model;

namespace Template10.Services.AdaptiveTiles
{
    public class AdaptiveTileService : IAdaptiveTileService
    {
        AdaptiveTileHelper _helper = new AdaptiveTileHelper();

        public void UpdatePrimaryTile(Tile tile)
        {
            _helper.UpdatePrimaryTile(tile);
        }

        public void UpdateSecondaryTile(Tile tile, string tileId)
        {
            _helper.UpdateSecondaryTile(tile, tileId);
        }
    }
}
