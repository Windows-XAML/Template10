using System;
using Template10.Core;
using Template10.Services.BackButtonService;
using Windows.UI.Xaml;

namespace Template10
{
    public static partial class Settings
    {
        public static ElementTheme DefaultTheme
        {
            get => Navigation.Settings.DefaultTheme;
            set => Navigation.Settings.DefaultTheme = value;
        }

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
