namespace Template10.Common.PersistedDictionary
{
    public static class Settings
    {
        public static Services.SerializationService.ISerializationService SerializationStrategy { get; set; }
            = Services.SerializationService.SerializationHelper.Json;
    }
}
