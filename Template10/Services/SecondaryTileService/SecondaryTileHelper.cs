using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;

namespace Template10.Services.SecondaryTileService
{
    public class SecondaryTileHelper
    {
        public bool IsPinned(string id)
        {
            return SecondaryTile.Exists(id);
        }

        public async Task<bool> PinAsync(SecondaryTileInfo info, string id)
        {
            System.Diagnostics.Contracts.Contract.Requires(info != null, "TileInfo");

            // already exists
            if (Exists(id))
                return true;

            var tile = new SecondaryTile()
            {
                TileId = id,
                DisplayName = info.DisplayName,
                Arguments = info.Arguments,
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

            if (info.VisualElements.Square30x30Logo != null)
            {
                tile.VisualElements.Square30x30Logo = info.VisualElements.Square30x30Logo;
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

        public bool Exists(string Id)
        {
            return SecondaryTile.Exists(Id);
        }

        public async Task<bool> UnpinAsync(string Id, Windows.UI.Popups.Placement placement = Windows.UI.Popups.Placement.Above)
        {
            System.Diagnostics.Contracts.Contract.Requires(Id != null, "TileId");
            if (!SecondaryTile.Exists(Id))
                return true;
            var tile = new SecondaryTile(Id);
            var result = await tile.RequestDeleteForSelectionAsync(new Rect(0, 0, 0, 0), placement);
            return result;
        }       
    }
}