using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Template10.Services.LocationService
{
    public interface ILocationService
    {
        Geoposition Position { get; }
    }
}
