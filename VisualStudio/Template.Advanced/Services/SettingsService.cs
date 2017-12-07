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
    public class SettingsService : SettingsServiceBase, ISettingsService
    {
        public SettingsService(ISettingsAdapter adapter, ISerializationService serial)
            : base(adapter, serial)
        {
            // empty
        }

        public ElementTheme DefaultTheme
        {
            get => Helper.SafeRead(nameof(DefaultTheme), ElementTheme.Light);
            set => Write(nameof(DefaultTheme), value.ToString());
        }

        public ShellBackButtonPreferences ShellBackButtonPreference
        {
            get => Helper.SafeRead(nameof(ShellBackButtonPreference), ShellBackButtonPreferences.AutoShowInShell);
            set => Write(nameof(ShellBackButtonPreference), value);
        }

        public string BusyText
        {
            get => Helper.SafeRead(nameof(BusyText), "Please wait...");
            set => Write(nameof(BusyText), value);
        }

        private T Read<T>(string key, T otherwise, [CallerMemberName] string propertyName = null)
        {
            this.Log($"{propertyName} Key:{key} Otherwise:{otherwise}");

            return Helper.SafeRead<T>(key, otherwise);
        }

        private void Write<T>(string key, T value, [CallerMemberName] string propertyName = null)
        {
            this.Log($"{propertyName} Key:{key} Value:{value}");

            // persist it

            if (!Helper.TryWrite(key, value))
            {
                System.Diagnostics.Debugger.Break();
            }

            // implement it

            WindowEx2.Current().Dispatcher.Dispatch(() =>
            {
                if (key == nameof(DefaultTheme))
                {
                    (Window.Current.Content as FrameworkElement).RequestedTheme = DefaultTheme;
                }
            });
        }
    }
}
