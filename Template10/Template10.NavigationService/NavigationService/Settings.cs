using System;
using Template10.Common.PersistedDictionary;
using Template10.Services.SerializationService;
using Template10.Strategies;

namespace Template10.Navigation
{
    public static partial class Settings
    {
        public static IPersistedDictionaryFactory PersistedDictionaryFactory { get; set; }
        public static IViewModelResolutionStrategy ViewModelResolutionStrategy { get; set; } = new DefaultViewModelResolutionStrategy();
        public static IViewModelActionStrategy ViewModelActionStrategy { get; set; } = new DefaultViewModelActionStrategy { SessionState = Common.SessionStateHelper.Current };
        public static ISerializationService SerializationStrategy { get; set; } = SerializationHelper.Json;
        public static bool RequireParameterSerialization { get; set; } = true;
        public static TimeSpan CacheMaxDuration { get; set; } = TimeSpan.MaxValue;
    }
}

