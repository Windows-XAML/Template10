using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template10.Services.MapService
{
    public interface IMapService
    {
        Task<List<MapHelper.Resource>> FindLocationByPointAsync(double latitude, double longitude);
        Uri GetMapUrl(IEnumerable<MapHelper.StaticMapPushpin> items, MapHelper.StaticMapPushpin user, MapHelper.StaticMapPushpin center, Windows.Foundation.Size size, int? zoom = null, MapHelper.StaticMapImagerySets imagery = MapHelper.StaticMapImagerySets.Road);
        Task<List<MapHelper.Resource>> FindLocationByQueryAsync(string query, int maxResults = 5);
    }
}
