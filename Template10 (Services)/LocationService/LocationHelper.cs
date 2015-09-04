using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;

namespace Template10.Services.LocationService
{
    class LocationHelper
    {
        private Geolocator _geolocator;
        public LocationHelper()
        {
            _geolocator = new Geolocator();
            _geolocator.PositionChanged += (s, e) =>
            {
                this.Position = e.Position;
                if (PositionChanged != null)
                {
                    try { PositionChanged(this.Position); }
                    catch { }
                }
            };

            // initial value
            this.Position = _geolocator.GetGeopositionAsync().AsTask<Geoposition>().Result;
        }

        public Windows.Devices.Geolocation.Geoposition Position { get; private set; }
        public Action<Geoposition> PositionChanged { get; set; }
    }
}
