using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;

namespace Template10.Services.LocationService
{
    public class LocationService : ILocationService
    {
        LocationHelper _helper = new LocationHelper();

        public Geoposition Position => _helper.Position;
    }
}
