using System;
using Template10.Common;
using Template10.Extensions;
using Template10.Services.Serialization;
using Template10.Strategies;
using Windows.UI.Xaml;
using Windows.Storage;
using System.Reflection;

namespace Template10.Navigation
{
    public enum ParameterBehaviors { Serialize, Raw }

    public enum NavigationBehaviors { Type, Key, Any }


    public static partial class Settings
    {
        public static ParameterBehaviors ParameterBehavior { get; set; } = ParameterBehaviors.Serialize;

        public static NavigationBehaviors NavigationBehavior { get; set; } = NavigationBehaviors.Any;

        public static PageKeyRegistry PageKeyRegistry { get; } = new PageKeyRegistry();

        public static TimeSpan CacheMaxDuration
        {
            get => ApplicationData.Current.LocalSettings.TryRead<TimeSpan>(nameof(CacheMaxDuration), out var value) ? value : TimeSpan.FromDays(2);
            set => ApplicationData.Current.LocalSettings.TryWrite(nameof(CacheMaxDuration), value);
        }

        public static DateTime LastSuspended
        {
            get => ApplicationData.Current.LocalSettings.TryRead<DateTime>(nameof(LastSuspended), out var value) ? value : DateTime.MaxValue;
            set => ApplicationData.Current.LocalSettings.TryWrite(nameof(LastSuspended), value);
        }
    }
}

