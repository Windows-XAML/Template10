using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace Template10.Services.PrimaryTileService
{
    public class PrimaryTileService : IPrimaryTileService
    {
        PrimaryTileHelper _helper = new PrimaryTileHelper();

        public void UpdateBadge(int value)
        {
            this._helper.UpdateBadge(value);
        }
    }

}
