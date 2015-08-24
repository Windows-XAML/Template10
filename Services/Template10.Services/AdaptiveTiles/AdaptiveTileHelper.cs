using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Template10.Services.AdaptiveTiles.Model;

namespace Template10.Services.AdaptiveTiles
{
    public class AdaptiveTileHelper
    {
        public void UpdatePrimaryTile(Tile tile)
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            TileNotification notification = new TileNotification(tile.ToXmlDocument());
            updater.Update(notification);
        }

        public void UpdateSecondaryTile(Tile tile, string tileId)
        {
            var updater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId);
            TileNotification notification = new TileNotification(tile.ToXmlDocument());
            updater.Update(notification);
        }
    }
}
