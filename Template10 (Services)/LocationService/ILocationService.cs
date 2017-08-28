using Windows.Devices.Geolocation;

namespace Template10.Services.LocationService
{
    public interface ILocationService
    {
        Geoposition Position { get; }
    }
}
