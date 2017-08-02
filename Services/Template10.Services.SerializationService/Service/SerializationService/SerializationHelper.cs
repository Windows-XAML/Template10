namespace Template10.Services.SerializationService
{
    public static class SerializationHelper
    {
        private static ISerializationService _json;
        public static ISerializationService Json => _json ?? (_json = new Strategies.JsonSerializationService());
    }
}