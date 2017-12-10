using System;
using Template10.Common;
using Template10.Navigation;
using Template10.Services.Gesture;
using Template10.Strategies;
using Windows.UI.Xaml;

namespace Template10
{
    public enum NetworkRequirements { None, Network, Internet }

    public enum SplashShowBehaviors { Auto, Manual }

    public enum SplashHideBehaviors { Auto, Manual }

    public enum ResumeBehaviors { Always, Never, Auto }

    public static partial class Settings
    {
        // navigation settings

        public static TimeSpan CacheMaxDuration
        {
            get => Navigation.Settings.CacheMaxDuration;
            set => Navigation.Settings.CacheMaxDuration = value;
        }

        public static ParameterBehaviors ParameterBehavior
        {
            get => Navigation.Settings.ParameterBehavior;
            set => Navigation.Settings.ParameterBehavior = value;
        }

        public static NavigationBehaviors NavigationBehavior
        {
            get => Navigation.Settings.NavigationBehavior;
            set => Navigation.Settings.NavigationBehavior = value;
        }

        /// <summary>
        /// What should happen to the SplashPopup at load is started?
        /// </summary>
        public static SplashShowBehaviors SplashShowBehavior { get; set; } = SplashShowBehaviors.Auto;

        /// <summary>
        /// What should happen to the SplashPopup at load is completed?
        /// </summary>
        public static SplashHideBehaviors SplashHideBehavior { get; set; } = SplashHideBehaviors.Auto;

        /// <summary>
        /// Every time the app starts, try to resume, ignore previous state
        /// </summary>
        public static ResumeBehaviors AppResumeBehavior { get; set; } = ResumeBehaviors.Auto;

        // services settings

        public static ShellBackButtonPreferences ShellBackButtonPreference
        {
            get => Services.Gesture.Settings.ShellBackButtonPreference;
            set => Services.Gesture.Settings.ShellBackButtonPreference = value;
        }
    }
}
