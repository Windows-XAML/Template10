using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;

namespace Template10.Services.LocationService
{
    class LocationService 
    {
        LocationHelper _helper = new LocationHelper();

        public Geoposition Position
        {
            get { return _helper.Position; }
        }
    }
}
