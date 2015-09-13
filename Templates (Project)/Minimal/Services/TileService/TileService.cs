using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace Minimal.Services.TileService
{
    public class TileService
    {
        public async Task<bool> IsPinned(ViewModels.DetailPageViewModel vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            var tileId = vm.GetType().ToString();
            var all = await SecondaryTile.FindAllAsync();
            return all.Any(x => x.TileId.Equals(tileId));
        }

        public async Task<bool> UnPinAsync(ViewModels.DetailPageViewModel vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));

            // already unpinned?
            if (!await IsPinned(vm))
                return true;

            try
            {
                var tile = new SecondaryTile(nameof(ViewModels.DetailPageViewModel));
                return await tile.RequestDeleteAsync();
            }
            catch (Exception)
            {
                System.Diagnostics.Debugger.Break();
                return false;
            }
        }

        public async Task<bool> PinAsync(ViewModels.DetailPageViewModel vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            try
            {
                // prep for custom tile
                var folder = await Windows.ApplicationModel.Package.Current
                    .InstalledLocation.GetFolderAsync("Services");
                folder = await folder.GetFolderAsync("TileService");
                var file = await folder.GetFileAsync("DetailTile.xml");
                var xml = XDocument.Load(file.Path);
                var visualElement = xml.Descendants("visual").First();
                var bindingElements = visualElement.Descendants("binding").ToArray();

                // medium
                {
                    // fetch targets
                    var bindingElement = bindingElements[0];

                    var titleElement = bindingElement.Descendants("text").First();
                    titleElement.SetValue("Detail page (medium tile)");

                    var subtitleElement = bindingElement.Descendants("text").ToArray()[1];
                    subtitleElement.SetValue(vm.Value);
                }

                // wide
                {
                    // fetch targets
                    var bindingElement = bindingElements[1];

                    var imageElement = bindingElement.Descendants("image").First();
                    var imagePath = new Uri("ms-appx://Services/ToastService/Template10.png");
                    imageElement.SetAttributeValue("src", imagePath.ToString());

                    var titleElement = bindingElement.Descendants("text").First();
                    titleElement.SetValue("Detail page (medium tile)");

                    var subtitleElement = bindingElement.Descendants("text").ToArray()[1];
                    subtitleElement.SetValue(vm.Value);
                }

                // large
                {
                    // fetch targets
                    var bindingElement = bindingElements[2];

                    var imageElement = bindingElement.Descendants("image").First();
                    var imagePath = new Uri("ms-appx://Services/ToastService/Template10.png");
                    imageElement.SetAttributeValue("src", imagePath.ToString());

                    var titleElement = bindingElement.Descendants("text").First();
                    titleElement.SetValue("Detail page (wide tile)");

                    var subtitleElement = bindingElement.Descendants("text").ToArray()[1];
                    subtitleElement.SetValue(vm.Value);
                }

                // only one tile for this vm
                var tileId = vm.GetType().ToString();

                // initial pin
                if (!await IsPinned(vm))
                {
                    var secondaryTile = new SecondaryTile(tileId)
                    {
                        Arguments = vm.Value,
                        DisplayName = "Detail page",
                        VisualElements =
                        {
                            Square150x150Logo = new Uri("ms-appx:///Assets/Logo.png"),
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
                var tileUpdater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId);
                var tileNotification = new TileNotification(xml.ToXmlDocument());
                tileUpdater.Update(tileNotification);
                return true;
            }
            catch (Exception)
            {
                System.Diagnostics.Debugger.Break();
                return false;
            }
        }
    }
}

namespace System
{
    public static class Extensions
    {
        public static XmlDocument ToXmlDocument(this XDocument x)
        {
            x.Declaration = new XDeclaration("1.0", "utf-8", "yes");
            var d = new XmlDocument();
            d.LoadXml(x.ToString());
            return d;
        }
    }
}