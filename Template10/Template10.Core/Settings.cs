using System;
using Template10.Core;
using Template10.Services.BackButtonService;

namespace Template10
{
    public static partial class Settings
    {
                public static IDispatcherEx DefaultDispatcher { get; set; }

        public static TimeSpan CacheExpiry
        {
            get => Navigation.Settings.CacheExpiry;
            set => Navigation.Settings.CacheExpiry = value;
        }

        public static ShellBackButtonPreferences ShellBackButtonPreference
        {
            get => Services.BackButtonService.Settings.ShellBackButtonPreference;
            set => Services.BackButtonService.Settings.ShellBackButtonPreference = value;
        }
    }
}
