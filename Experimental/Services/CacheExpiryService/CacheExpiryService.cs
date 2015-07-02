using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services
{
    class CacheExpiryService
    {
        CacheExpiryHelper _helper;

        public CacheExpiryService()
        {
            this._helper = new CacheExpiryHelper();
        }
    }
}
