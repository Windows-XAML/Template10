﻿using System;
using Template10.Core;
using Template10.Navigation;
using Template10.Services.Container;
using Template10.Services.Gesture;
using Template10.Services.Serialization;
using Template10.Services.Settings;
using Windows.UI.Xaml;

namespace Sample.Services
{
    public class SettingsService : SettingsServiceBase
    {
        private static SettingsService _instance;
        public static SettingsService GetInstance() => _instance;
        static SettingsService()
        {
            _instance = new SettingsService();
        }
        private SettingsService()
            : base(Windows.Storage.ApplicationData.Current.LocalSettings, SerializationService)
        {
            // empty
        }

        public ElementTheme DefaultTheme
        {
            get => Read(nameof(DefaultTheme), ElementTheme.Light);
            set => Write(nameof(DefaultTheme), value.ToString());
        }

        public TimeSpan CacheMaxDuration
        {
            get => Read(nameof(CacheMaxDuration), TimeSpan.FromHours(1));
            set => Write(nameof(CacheMaxDuration), value);
        }

        public string BusyText
        {
            get => Read(nameof(BusyText), "Please wait...");
            set => Write(nameof(BusyText), value);
        }

        public ShellBackButtonPreferences ShellBackButtonPreference
        {
            get => Read(nameof(ShellBackButtonPreference), ShellBackButtonPreferences.AutoShowInShell);
            set => Write(nameof(ShellBackButtonPreference), value);
        }

        private new void Write<T>(string key, T value)
        {
            // persist it

            if (!TryWrite(key, value))
            {
                System.Diagnostics.Debugger.Break();
            }

            // implement it

            WindowEx.Current().Dispatcher.Dispatch(() =>
            {
                switch (key)
                {
                    case nameof(ShellBackButtonPreference):
                        // hide/show let the service handle it
                        BackButtonService.UpdateBackButton(NavigationService.Default.CanGoBack);
                        break;
                    case nameof(DefaultTheme):
                        // update the requested theme
                        (Window.Current.Content as FrameworkElement).RequestedTheme = DefaultTheme;
                        break;
                    case nameof(CacheMaxDuration):
                        // update the navigation setting
                        Template10.Navigation.Settings.CacheMaxDuration = CacheMaxDuration;
                        break;
                }
            });
        }

        public static IContainerService ContainerService => Template10.Services.Container.ContainerService.Default;
        public static IBackButtonService BackButtonService => ContainerService.Resolve<IBackButtonService>();
        public static ISerializationService SerializationService => ContainerService.Resolve<ISerializationService>();
    }
}
