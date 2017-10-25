using System;
using Template10.Services.Dependency;
using Template10.Services.Serialization;
using Template10.Core;
using Template10.Extensions;
using Template10.Navigation;
using Template10.Services.Gesture;
using Template10.Services.Settings;
using Windows.UI.Xaml;

namespace Sample.Services
{
    public class SettingsService : SettingsServiceBase, ISettingsService
    {
        public SettingsService()
            : base(Windows.Storage.ApplicationData.Current.LocalSettings)
        {
            // empty
        }

        public ElementTheme DefaultTheme
        {
            get => Read(nameof(DefaultTheme), ElementTheme.Light);
            set => Write(nameof(DefaultTheme), value.ToString());
        }

        public ShellBackButtonPreferences ShellBackButtonPreference
        {
            get => Read(nameof(ShellBackButtonPreference), ShellBackButtonPreferences.AutoShowInShell);
            set => Write(nameof(ShellBackButtonPreference), value);
        }

        public string BusyText
        {
            get => Read(nameof(BusyText), "Please wait...");
            set => Write(nameof(BusyText), value);
        }

        private new void Write<T>(string key, T value)
        {
            this.Log($"Key:{key} Value:{value}");

            // persist it

            if (!TryWrite(key, value))
            {
                System.Diagnostics.Debugger.Break();
            }

            // implement it

            WindowEx.Current().Dispatcher.Dispatch(() =>
            {
                if (key == nameof(DefaultTheme))
                {
                    (Window.Current.Content as FrameworkElement).RequestedTheme = DefaultTheme;
                }
            });
        }
    }
}
