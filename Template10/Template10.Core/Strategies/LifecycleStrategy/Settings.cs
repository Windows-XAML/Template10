using System;
using Template10.Extensions;
using Windows.Storage;

namespace Template10.Strategies
{
    public static partial class Settings
    {
        internal static DateTime LastSuspended
        {
            get => ApplicationData.Current.LocalSettings.TryRead<DateTime>(nameof(LastSuspended), out var value) ? value : DateTime.MaxValue;
            set => ApplicationData.Current.LocalSettings.TryWrite(nameof(LastSuspended), value);
        }

        /// <summary>
        /// Every time the app starts, try to resume, ignore previous state
        /// </summary>
        public static bool AppAlwaysResumes { get; set; } = false;

        public static bool EnableLifecycleStrategy { get; set; } = true;
    }
}
