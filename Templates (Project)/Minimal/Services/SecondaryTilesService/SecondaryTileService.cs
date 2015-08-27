using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Minimal.Services.SecondaryTilesService
{
    public class SecondaryTileService
    {
        public void PinDetailPage(string value)
        {
            var uri = new Uri("ms-appx:///Templates/DetailTile.xml");
            var template = XDocument.Load(uri.ToString());
            var visual = template.Descendants("visual").First();

            // medium
            //var binding = visual.Descendants("binding")[0];
        }
    }
}
