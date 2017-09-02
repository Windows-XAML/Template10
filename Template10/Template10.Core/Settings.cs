using System;
using Template10.Core;
using Template10.Services.Gesture;
using Template10.Strategies;
using Windows.UI.Xaml;

namespace Template10
{
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

        public static NetworkRequirements NetworkRequirement
        {
            get => Strategies.Settings.NetworkRequirement;
            set => Strategies.Settings.NetworkRequirement = value;
        }

        public static bool ShowExtendedSplashScreen
        {
            get => Strategies.Settings.ShowExtendedSplashScreen;
            set => Strategies.Settings.ShowExtendedSplashScreen = value;
        }

        public static bool AppAlwaysResumes
        {
            get => Strategies.Settings.AppAlwaysResumes;
            set => Strategies.Settings.AppAlwaysResumes = value;
        }

        public static bool EnableCustomTitleBar
        {
            get => Strategies.Settings.EnableCustomTitleBar;
            set => Strategies.Settings.EnableCustomTitleBar = value;
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
