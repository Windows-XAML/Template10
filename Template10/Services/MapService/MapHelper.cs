using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Template10.Services.MapService
{
    // new Geolocator().GetPositionAsunc();
    // var x = new MapHelper("key").FindLocatioByX(postalCode:80433);

    /// <summary>Search Bing Maps</summary>
    /// <see cref="http://msdn.microsoft.com/en-us/library/ff701715">Reference</see>
    /// <see cref="https://www.bingmapsportal.com">Token</see>
    public class MapHelper
    {
        string _bingMapsKey;
        public MapHelper(string bingMapsKey)
        {
            _bingMapsKey = bingMapsKey;
        }

        public class StaticMapPushpin
        {
            public StaticMapPushpin()
            {
                Latitude = 0d;
                Longitude = 0d;
                Text = "?";
                IconStyle = StaticMapIconStyles.LargeOrangeDot;
            }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string Text { get; set; }
            public StaticMapIconStyles IconStyle { get; set; }
        }

        public enum StaticMapIconStyles : int
        {
            // http://msdn.microsoft.com/en-us/library/ff701719.aspx
            Star = 0,
            BlueBox = 1,
            RedBox = 7,
            OrangeBox = 23,
            OrangeBoxWithArrow = 24,
            PurpleBox = 27,
            SmallGreenDot = 33,
            LargeOrangeDot = 34,
            GreenFlag = 35,
            LargeGreenDot = 36,
        }

        public enum StaticMapImagerySets
        {
            Aerial, // Aerial imagery
            AerialWithLabels, // Aerial imagery with a road overlay.
            Road, // Roads without additional imagery.
            OrdnanceSurvey, // Ordnance Survey imagery.
            CollinsBart, // Collins Bart imagery.
        }

        /// <summary>
        /// Returns map URL
        /// </summary>
        /// <see cref="http://msdn.microsoft.com/en-us/library/ff701724.aspx">Documentation</see>
        /// <param name="items"></param>
        /// <param name="user"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Uri GetMapUrl(IEnumerable<StaticMapPushpin> items, StaticMapPushpin user, StaticMapPushpin center, Size size, int? zoom = null, StaticMapImagerySets imagery = StaticMapImagerySets.Road)
        {
            if (size == null)
                throw new ArgumentNullException("size");

            var _Builder = new StringBuilder();

            var _Url = string.Format("http://dev.virtualearth.net/REST/v1/Imagery/Map/{0}?key={1}", imagery, _bingMapsKey);
            _Builder.Append(_Url);

            var _Size = string.Format("&mapSize={0},{1}", size.Width, size.Height);
            _Builder.Append(_Size);

            var _Zoom = string.Format("&zoomLevel={0}", zoom);
            _Builder.Append(_Zoom);

            // optional is okay
            if (center != null)
            {
                var _Center = string.Format("&centerPoint={0},{1}", center.Latitude, center.Longitude);
                _Builder.Append(_Center);
            }

            // optional is okay
            if (user != null)
            {
                var _User = string.Format("&pp={0},{1};{2};{3}", user.Latitude, user.Longitude, (int)user.IconStyle, user.Text);
                _Builder.Append(_User);
            }

            // optional is okay
            if (items != null)
            {
                var _MaxItems = 17;
                var _Items = items.Take(_MaxItems).Select(x => string.Format("&pp={0},{1};{2};{3}", x.Latitude, x.Longitude, (int)x.IconStyle, x.Text)).ToArray();
                _Builder.Append(string.Join(string.Empty, _Items));
            }

            return new Uri(_Builder.ToString());
        }

        /// <summary>Use the following URL template to get the location information associated with latitude and longitude coordinates.</summary>
        /// <param name="latitude">The coordinates of the location that you want to reverse geocode. A point is specified by a latitude and a longitude.</param>
        /// <param name="longitude">The coordinates of the location that you want to reverse geocode. A point is specified by a latitude and a longitude.</param>
        /// <see cref="http://msdn.microsoft.com/en-us/library/ff701710"/>
        /// <returns>When you make a request by using the following URL template, the response returns one or more Location resources that contain location information associated with the latitude and longitude coordinate values that you specify. Location information can be as specific as an address or more general such as the country or region.</returns>
        public async Task<List<Resource>> FindLocationByPointAsync(double latitude, double longitude)
        {
            var _Template = "http://dev.virtualearth.net/REST/v1/Locations/{0},{1}?output=JSON&includeEntityTypes=Address,Neighborhood,PopulatedPlace,Postcode1,AdminDivision1,AdminDivision2,CountryRegion&includeNeighborhood=1&key={2}";
            var _Url = string.Format(_Template, latitude, longitude, _bingMapsKey);
            var _Result = await CallService(new Uri(_Url));
            if (_Result == null)
                return null;
            return _Result.resourceSets[0].resources;
        }

        /// <summary>Use the following URL templates to get latitude and longitude coordinates that correspond to location information provided as a query string. The strings Space Needle (a landmark) and 1 Microsoft Way Redmond WA (an address) are examples of query strings with location information. These strings can be specified as a structured URL parameter or as a query parameter value.</summary>
        /// <param name="query">A string that contains information about a location, such as an address or landmark name.</param>
        /// <param name="includeNeighborhood">Specifies to include the neighborhood with the address information the response when it is available.</param>
        /// <param name="maxResults">Specifies the maximum number of locations to return in the response. An integer between 1 and 20. The default value is 5.</param>
        /// <see cref="http://msdn.microsoft.com/en-us/library/ff701711.aspx"/>
        /// <returns>When you make a request by using one of the following URL templates, the response returns one or more Location resources that contain location information associated with the URL parameter values. The location information for each resource includes latitude and longitude coordinates, the type of location, and the geographical area that contains the location.</returns>
        public async Task<List<Resource>> FindLocationByQueryAsync(string query, int maxResults = 5)
        {
            var _Template = "http://dev.virtualearth.net/REST/v1/Locations?output=JSON&query={0}&includeNeighborhood=1&include=queryParse&maxResults={1}&key={2}";
            var _Url = string.Format(_Template, query, maxResults, _bingMapsKey);
            var _Result = await CallService(new Uri(_Url));
            if (_Result == null)
                return null;
            return _Result.resourceSets[0].resources;
        }

        /// <summary>Use the following URL templates to get latitude and longitude coordinates for a location by specifying values such as a locality, postal code, and street address.</summary>
        /// <param name="country">A string specifying the ISO country code.</param>
        /// <param name="adminDistrict">A string that contains a subdivision, such as the abbreviation of a US state.</param>
        /// <param name="locality">A string that contains the locality, such as a US city.</param>
        /// <param name="postalCode">A string that contains the postal code, such as a US ZIP Code.</param>
        /// <param name="addressLine">A string specifying the street line of an address.</param>
        /// <param name="userLocation">A point on the earth specified as a latitude and longitude. When you specify this parameter, the user’s location is taken into account and the results returned may be more relevant to the user.</param>
        /// <param name="userIP">The default address is the IPv4 address of the request. When you specify this parameter, the location associated with the IP address is taken into account in computing the results of a location query.</param>
        /// <param name="maxResults">A string that contains an integer between 1 and 20. The default value is 5.</param>
        /// <see cref="http://msdn.microsoft.com/en-us/library/ff701714"/>
        /// <returns>When you make a request by using one of the following URL templates, the response returns one or more Location resources that contain location information associated with the URL parameter values.</returns>
        public async Task<List<Resource>> FindLocationByAddressAsync(Countries country = Countries.US, string adminDistrict = null, string locality = null, string postalCode = null, string addressLine = null, string userLocation = null, string userIP = null, int maxResults = 5)
        {
            var _Template = "http://dev.virtualearth.net/REST/v1/Locations?output=JSON&countryRegion={0}&adminDistrict={1}&locality={2}&postalCode={3}&addressLine={4}&userLocation={5}&userIp={6}&includeNeighborhood=1&maxResults={7}&key={8}";
            var _Url = string.Format(_Template, country.ToString(), adminDistrict, locality, postalCode, addressLine, userLocation, userIP, maxResults, _bingMapsKey);
            var _Result = await CallService(new Uri(_Url));
            if (_Result == null)
                return null;
            return _Result.resourceSets[0].resources;
        }

        // helper method
        private static async Task<RootObject> CallService(Uri uri)
        {
            string _JsonString = string.Empty;
            try
            {
                // fetch from rest service
                var _HttpClient = new System.Net.Http.HttpClient();
                var _HttpResponse = await _HttpClient.GetAsync(uri.ToString());
                _JsonString = await _HttpResponse.Content.ReadAsStringAsync();

                // check for error
                if (!_JsonString.Contains("\"statusCode\":200"))
                    throw new InvalidStatusCodeException { JSON = _JsonString };
                if (_JsonString.Contains("InvalidCredentials")
                    || _JsonString.Contains("CredentialsExpired")
                    || _JsonString.Contains("NotAuthorized")
                    || _JsonString.Contains("NoCredentials"))
                    throw new InvalidCredentialsException { JSON = _JsonString };

                // deserialize json to objects
                var _JsonBytes = Encoding.Unicode.GetBytes(_JsonString);
                using (MemoryStream _MemoryStream = new MemoryStream(_JsonBytes))
                {
                    var _JsonSerializer = new DataContractJsonSerializer(typeof(RootObject));
                    var _Result = (RootObject)_JsonSerializer.ReadObject(_MemoryStream);
                    return _Result;
                }
            }
            catch (InvalidSomethingException) { throw; }
            catch (Exception e) { throw new InvalidSomethingException(e) { JSON = _JsonString }; }
        }

        public class InvalidSomethingException : Exception
        {
            public InvalidSomethingException() { }
            public InvalidSomethingException(Exception e) : base(string.Empty, e) { }
            public string JSON { get; set; }
        }
        public class InvalidStatusCodeException : InvalidSomethingException { }
        public class InvalidCredentialsException : InvalidSomethingException { }

        public enum Countries
        {
            US, UK, FR, DE, CA
        }

        [DataContract]
        public class Point
        {
            [DataMember]
            public string type { get; set; }
            [DataMember]
            public List<double> coordinates { get; set; }
        }

        [DataContract]
        public class Address
        {
            [DataMember]
            public string addressLine { get; set; }
            [DataMember]
            public string adminDistrict { get; set; }
            [DataMember]
            public string adminDistrict2 { get; set; }
            [DataMember]
            public string countryRegion { get; set; }
            [DataMember]
            public string formattedAddress { get; set; }
            [DataMember]
            public string locality { get; set; }
            [DataMember]
            public string postalCode { get; set; }
        }

        [DataContract]
        public class GeocodePoint
        {
            [DataMember]
            public string type { get; set; }
            [DataMember]
            public List<double> coordinates { get; set; }
            [DataMember]
            public string calculationMethod { get; set; }
            [DataMember]
            public List<string> usageTypes { get; set; }
        }

        [System.Runtime.Serialization.DataContract(Namespace = "http://schemas.microsoft.com/search/local/ws/rest/v1", Name = "Location")]
        public class Resource
        {
            [DataMember]
            public string __type { get; set; }
            [DataMember]
            public List<double> bbox { get; set; }
            [DataMember]
            public string name { get; set; }
            [DataMember]
            public Point point { get; set; }
            [DataMember]
            public Address address { get; set; }
            [DataMember]
            public string confidence { get; set; }
            [DataMember]
            public string entityType { get; set; }
            [DataMember]
            public List<GeocodePoint> geocodePoints { get; set; }
            [DataMember]
            public List<string> matchCodes { get; set; }
        }

        [DataContract]
        public class ResourceSet
        {
            [DataMember]
            public int estimatedTotal { get; set; }
            [DataMember]
            public List<Resource> resources { get; set; }
        }

        [DataContract]
        public class RootObject
        {
            [DataMember]
            public string authenticationResultCode { get; set; }
            [DataMember]
            public string brandLogoUri { get; set; }
            [DataMember]
            public string copyright { get; set; }
            [DataMember]
            public List<ResourceSet> resourceSets { get; set; }
            [DataMember]
            public int statusCode { get; set; }
            [DataMember]
            public string statusDescription { get; set; }
            [DataMember]
            public string traceId { get; set; }
        }
    }


}
