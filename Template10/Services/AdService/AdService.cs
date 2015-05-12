using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.AdService
{
    class AdService
    {
        AdHelper _helper;
        public AdService()
        {
            // for demo only
            var adUnitId = "11389925";
            var appId = "d25517cb-12d4-4699-8bdc-52040c712cab";
            _helper = new AdHelper(appId, adUnitId);
            _helper.Preload();
        }

        public void Show(Action<AdHelper.Results> callback)
        {
            _helper.Show(callback);
        }
    }
}
