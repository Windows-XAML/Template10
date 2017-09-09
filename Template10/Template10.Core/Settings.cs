using System;
using Template10.Core;
using Template10.Services.Gesture;
using Template10.Strategies;
using Windows.UI.Xaml;

namespace Template10
{
    public enum NetworkRequirements { None, NetworkRequired, InternetRequired }

    public static partial class Settings
    {
        // navigation settings

        public static ElementTheme DefaultTheme
        {
            get => Navigation.Settings.DefaultTheme;
            set => Navigation.Settings.DefaultTheme = value;
        }

        public static TimeSpan CacheMaxDuration
        {
            get => Navigation.Settings.CacheMaxDuration;
            set => Navigation.Settings.CacheMaxDuration = value;
        }

        public static bool RequireSerializableParameters
        {
            get => Navigation.Settings.RequireSerializableParameters;
            set => Navigation.Settings.RequireSerializableParameters = value;
        }

        // strategy settings

        public static NetworkRequirements NetworkRequirement { get; set; } = NetworkRequirements.None;

        public static bool ShowExtendedSplashScreen { get; set; } = true;
        public static bool AutoHideExtendedSplashScreen { get; set; } = true;

        public static bool AppAlwaysResumes
        {
            get => Strategies.Settings.AppAlwaysResumes;
            set => Strategies.Settings.AppAlwaysResumes = value;
        }

        public static bool EnableExtendedSessionStrategy
        {
            get => Strategies.Settings.EnableExtendedSessionStrategy;
            set => Strategies.Settings.EnableExtendedSessionStrategy = value;
        }

        public static bool EnableLifecycleStrategy
        {
            get => Strategies.Settings.EnableLifecycleStrategy;
            set => Strategies.Settings.EnableLifecycleStrategy = value;
        }

        // services settings

        public static ShellBackButtonPreferences ShellBackButtonPreference
        {
            get => Services.Gesture.Settings.ShellBackButtonPreference;
            set => Services.Gesture.Settings.ShellBackButtonPreference = value;
        }
    }
}
