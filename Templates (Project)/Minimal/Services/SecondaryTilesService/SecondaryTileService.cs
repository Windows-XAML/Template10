using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace Minimal.Services.SecondaryTilesService
{
    public class SecondaryTileService
    {
        public bool IsPinned(ViewModels.DetailPageViewModel vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            return SecondaryTile.Exists(nameof(ViewModels.DetailPageViewModel));
        }

        public bool UnPin(ViewModels.DetailPageViewModel vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            if (!IsPinned(vm))
                return true;
            try
            {
                var tile = new SecondaryTile(nameof(ViewModels.DetailPageViewModel));
                tile.RequestDeleteAsync().AsTask().Wait();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Pin(ViewModels.DetailPageViewModel vm)
        {
            if (vm == null)
                throw new ArgumentNullException(nameof(vm));
            if (IsPinned(vm))
                return true;
            try
            {
                // prep for custom
                var uri = new Uri("ms-appx:///Templates/DetailTile.xml");
                var xml = XDocument.Load(uri.ToString());
                var visual = xml.Descendants("visual").First();
                var bindings = visual.Descendants("binding").ToArray();

                // medium
                {
                    var binding = bindings[0];
                    var texts = binding.Descendants("text").ToArray();
                    var title = texts[0];
                    var subtitle = texts[0];

                    title.Value = "Detail page";
                    subtitle.Value = string.Format("Pass '{0}'.", vm.Value);
                }

                // wide
                {
                    var binding = bindings[0];
                    var subgroups = binding.Descendants("subgroup").ToArray();
                    var image = subgroups[0].Descendants("image").First();
                    var texts = subgroups[1].Descendants("text").ToArray();
                    var title = texts[0];
                    var subtitle = texts[0];

                    image.Value = new Uri("ms-appx://assets/logo.png").ToString();
                    title.Value = "Detail page (medium tile)";
                    subtitle.Value = string.Format("Pass '{0}'.", vm.Value);
                }

                // large
                {
                    var binding = bindings[0];
                    var subgroups = binding.Descendants("subgroup").ToArray();
                    var image = subgroups[0].Descendants("image").First();
                    var texts = subgroups[1].Descendants("text").ToArray();
                    var title = texts[0];
                    var subtitle = texts[0];

                    image.Value = new Uri("ms-appx://assets/logo.png").ToString();
                    title.Value = "Detail page (wide tile)";
                    subtitle.Value = string.Format("Pass '{0}'.", vm.Value);
                }

                // pin
                var updater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(nameof(ViewModels.DetailPageViewModel));
                var notification = new TileNotification(xml.ToXmlDocument());
                updater.Update(notification);

                return true;
            }
            catch (Exception)
            {
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