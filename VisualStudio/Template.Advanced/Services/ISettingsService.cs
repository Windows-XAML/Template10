using System;
using Template10.Services.Gesture;
using Windows.UI.Xaml;

namespace Sample.Services
{
    public interface ISettingsService
    {
        string BusyText { get; set; }
        TimeSpan CacheMaxDuration { get; set; }
        ElementTheme DefaultTheme { get; set; }
        ShellBackButtonPreferences ShellBackButtonPreference { get; set; }
    }
}