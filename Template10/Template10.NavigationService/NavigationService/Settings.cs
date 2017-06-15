using System;
using Template10.Common.PersistedDictionary;
using Template10.Services.SerializationService;

namespace Template10.Services.NavigationService
{
    public static class Settings
    {
        public static PersistedDictionaryTypes PersistenceDefault { get; set; } = PersistedDictionaryTypes.FileSystem;
        public static IViewModelResolutionStrategy ViewModelResolutionStrategy { get; set; } = new DefaultViewModelResoltionStrategy();
        public static IViewModelActionStrategy ViewModelActionStrategy { get; set; } = new PortableViewModelActionStrategy { SessionState = SessionState.Current };
        public static ISerializationService SerializationStrategy { get; set; } = Services.SerializationService.SerializationService.Json;
        public static TimeSpan CacheMaxDuration { get; set; } = TimeSpan.MaxValue;
    }
}

