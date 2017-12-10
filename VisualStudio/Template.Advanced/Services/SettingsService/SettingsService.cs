using System;
using Template10.Services.DependencyInjection;
using Template10.Services.Serialization;
using Template10.Common;
using Template10.Extensions;
using Template10.Navigation;
using Template10.Services.Gesture;
using Template10.Services.Settings;
using Windows.UI.Xaml;
using System.Runtime.CompilerServices;

namespace Sample.Services
{
    public interface ISettingsService
    {
        string BusyText { get; set; }
        ShellBackButtonPreferences ShellBackButtonPreference { get; set; }
        ElementTheme DefaultTheme { get; set; }
    }

    public class SettingsService : ISettingsService
    {
        private static SettingsHelper _settingsHelper;

        static SettingsService()
        {
            var serializer = Template10.Central.Serialization;
            var settingsAdapter = new LocalFileSettingsAdapter(serializer);
            _settingsHelper = new SettingsHelper(settingsAdapter);
        }

        // public

        public ElementTheme DefaultTheme
        {
            get => _settingsHelper.SafeReadEnum(nameof(DefaultTheme), ElementTheme.Light);
            set
            {
                _settingsHelper.WriteEnum(nameof(DefaultTheme), value);
                ApplyDefaultTheme(value);
            }
        }

        private void ApplyDefaultTheme(ElementTheme value)
        {
            WindowExManager.Current().Dispatcher.Dispatch(() =>
            {
                (Window.Current.Content as FrameworkElement).RequestedTheme = value;
            });
        }

        public ShellBackButtonPreferences ShellBackButtonPreference
        {
            get => _settingsHelper.SafeReadEnum(nameof(ShellBackButtonPreference), ShellBackButtonPreferences.AutoShowInShell);
            set
            {
                _settingsHelper.WriteEnum(nameof(ShellBackButtonPreference), value);
                ApplyShellBackButtonPreference(value);
            }
        }

        // internal

        private void ApplyShellBackButtonPreference(ShellBackButtonPreferences value)
        {
            WindowExManager.Current().Dispatcher.Dispatch(() =>
            {
                Template10.Services.Gesture.Settings.ShellBackButtonPreference = value;
            });
        }

        public string BusyText
        {
            get => _settingsHelper.SafeRead(nameof(BusyText), "Please wait...");
            set => _settingsHelper.WriteString(nameof(BusyText), value);
        }
    }
}
