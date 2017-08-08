using System;
using Template10.Common;
using Template10.Extensions;
using Template10.Core;
using Template10.Services.Serialization;
using Template10.Strategies;

namespace Template10.Navigation
{
    public static partial class Settings
    {
        public static bool SerializeParameters { get; set; } = true;

        public static TimeSpan CacheExpiry
        {
            get => Windows.Storage.ApplicationData.Current.LocalSettings.TryRead<TimeSpan>(nameof(CacheExpiry), out var value) ? value : TimeSpan.FromDays(2);
            set => Windows.Storage.ApplicationData.Current.LocalSettings.TryWrite(nameof(CacheExpiry), value);
        }
    }
}

