using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace Template10.Services.PrimaryTileService
{
    class PrimaryTileService
    {
        PrimaryTileHelper _Helper = new PrimaryTileHelper();

        public void UpdateBadge(int value)
        {
            this._Helper.UpdateBadge(value);
        }
    }

}
