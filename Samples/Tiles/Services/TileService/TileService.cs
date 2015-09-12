using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.ViewModels;
using Windows.UI.StartScreen;
using AdaptiveTileExtensions;
using Windows.UI.Notifications;

namespace Sample.Services.TileService
{
    class TileService
    {
        internal async Task<bool> IsPinned(DetailPageViewModel detailPageViewModel)
        {
            var tileId = detailPageViewModel.ToString();
            return (await SecondaryTile.FindAllAsync()).Any(x => x.TileId.Equals(tileId));
        }

        internal async Task<bool> PinAsync(DetailPageViewModel detailPageViewModel)
        {
            // prepare content

            var header = new Text("Template 10")
            {
                Style = TextStyle.Subtitle
            };

            var content = new Text(detailPageViewModel.Value)
            {
                Style = TextStyle.Caption,
                WrapText = true
            };

            var logo = new TileImage(ImagePlacement.Inline)
            {
                Source = "https://raw.githubusercontent.com/Windows-XAML/Template10/master/Assets/Template10.png"
            };

            // build tile

            var tile = AdaptiveTile.CreateTile(string.Empty);
            var binding = TileBinding.Create(TemplateType.TileWide);
            binding.Branding = Branding.Name;

            var sub1 = new SubGroup { Width = 33 };
            binding.Add(sub1);
            sub1.AddImage(logo);

            var sub2 = new SubGroup();
            binding.Add(sub2);
            sub2.AddText(header);
            sub2.AddText(content);

            tile.Tiles.Add(binding);

            // show tile

            var tileId = detailPageViewModel.ToString();

            if (!await IsPinned(detailPageViewModel))
            {
                // initial pin
                var secondaryTile = new SecondaryTile(tileId)
                {
                    Arguments = detailPageViewModel.Value,
                    DisplayName = "Detail page",
                    VisualElements =
                        {
                            Square44x44Logo = new Uri("ms-appx:///Assets/Logo.png"),
                            Square150x150Logo = new Uri("ms-appx:///Assets/Logo.png"),
                            Wide310x150Logo = new Uri("ms-appx:///Assets/Logo.png"),
                            Square310x310Logo = new Uri("ms-appx:///Assets/Logo.png"),
                            ShowNameOnSquare150x150Logo = true,
                        },
                };
                if (!await secondaryTile.RequestCreateAsync())
                {
                    System.Diagnostics.Debugger.Break();
                    return false;
                }
            }

            // update pin
            var xml = tile.GetNotification().Content;
            xml.DocumentElement.RemoveAttribute("version");
            var value = xml.GetXml();
            var tileUpdater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId);
            var tileNotification = new TileNotification(xml);
            tileUpdater.Update(tileNotification);

            return true;
        }

        internal async Task<bool> UnPinAsync(DetailPageViewModel detailPageViewModel)
        {
            if (!await IsPinned(detailPageViewModel))
                return true;
            try
            {
                var tileId = detailPageViewModel.ToString();
                var tile = new SecondaryTile(tileId);
                return await tile.RequestDeleteAsync();
            }
            catch (Exception)
            {
                System.Diagnostics.Debugger.Break();
                return false;
            }
        }
    }
}
