using Template10.Services.AdaptiveTiles.Model;
using Windows.UI.Notifications;

namespace Template10.Services.AdaptiveTiles
{
    public class AdaptiveTileHelper
    {
		public void SetDefaultPrimaryTile()
		{
			UpdatePrimaryTile(new Tile());
		}
		
        public void UpdatePrimaryTile(Tile tile)
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            TileNotification notification = new TileNotification(tile.ToXmlDocument());
            updater.Update(notification);
        }

		public void SetDefaultSecondaryTile(string tileId)
		{
			UpdateSecondaryTile(new Tile(), tileId);
		}

		public void UpdateSecondaryTile(Tile tile, string tileId)
        {
            var updater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId);
            TileNotification notification = new TileNotification(tile.ToXmlDocument());
            updater.Update(notification);
        }
    }
}
