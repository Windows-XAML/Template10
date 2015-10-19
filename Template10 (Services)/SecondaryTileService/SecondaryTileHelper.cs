using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using System.Linq;

namespace Template10.Services.SecondaryTileService
{
    public class SecondaryTileHelper
    {
        public async Task<bool> PinAsync(TileInfo info, string tileId, string arguments)
        {
            System.Diagnostics.Contracts.Contract.Requires(info != null, "TileInfo");
            System.Diagnostics.Contracts.Contract.Requires(!string.IsNullOrEmpty(tileId), "TileId");

            if (Exists(tileId))
            {
                var existings = await SecondaryTile.FindAllAsync();
                var existing = existings.FirstOrDefault(x => x.Arguments.Equals(arguments));
            }

            var tile = new SecondaryTile()
            {
                TileId = tileId,
                DisplayName = info.DisplayName,
                Arguments = arguments,
                PhoneticName = info.PhoneticName,
                LockScreenDisplayBadgeAndTileText = info.LockScreenDisplayBadgeAndTileText,
            };

            if (info.LockScreenBadgeLogo != null)
            {
                tile.LockScreenBadgeLogo = info.LockScreenBadgeLogo;
            }

            tile.VisualElements.BackgroundColor = info.VisualElements.BackgroundColor;
            tile.VisualElements.ForegroundText = info.VisualElements.ForegroundText;
            tile.VisualElements.ShowNameOnSquare150x150Logo = info.VisualElements.ShowNameOnSquare150x150Logo;
            tile.VisualElements.ShowNameOnSquare310x310Logo = info.VisualElements.ShowNameOnSquare310x310Logo;
            tile.VisualElements.ShowNameOnWide310x150Logo = info.VisualElements.ShowNameOnWide310x150Logo;

            if (info.VisualElements.Square150x150Logo != null)
            {
                tile.VisualElements.Square150x150Logo = info.VisualElements.Square150x150Logo;
            }

            if (info.VisualElements.Square310x310Logo != null)
            {
                tile.VisualElements.Square310x310Logo = info.VisualElements.Square310x310Logo;
            }

            if (info.VisualElements.Wide310x150Logo != null)
            {
                tile.VisualElements.Wide310x150Logo = info.VisualElements.Wide310x150Logo;
            }

            var result = await tile.RequestCreateForSelectionAsync(info.Rect(), info.RequestPlacement);
            return result;
        }

        public bool Exists(string tileId) => SecondaryTile.Exists(tileId);

        public async Task<bool> UnpinAsync(string tileId, Rect selection, Windows.UI.Popups.Placement placement = Windows.UI.Popups.Placement.Above)
        {
            System.Diagnostics.Contracts.Contract.Requires(tileId != null, "TileId");

            if (!SecondaryTile.Exists(tileId))
                return true;
            var tile = new SecondaryTile(tileId);
            var result = await tile.RequestDeleteForSelectionAsync(selection, placement);
            return result;
        }

        public async Task<bool> UnpinAsync(string tileId)
        {
            System.Diagnostics.Contracts.Contract.Requires(tileId != null, "TileId");
            if (!SecondaryTile.Exists(tileId))
                return true;
            var tile = new SecondaryTile(tileId);
            return await tile.RequestDeleteAsync();
        }
    }
}
