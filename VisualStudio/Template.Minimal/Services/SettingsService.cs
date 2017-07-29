using System;
using Template10.Core;
using Template10.Extensions;
using Template10.Navigation;
using Template10.Services.BackButtonService;
using Template10.Services.SettingsService.Services.SettingsService;
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
        {
            // empty
        }

        public bool UseShellBackButton
        {
            get => Read(nameof(UseShellBackButton), true);
            set => Write(nameof(UseShellBackButton), value);
        }

        public ApplicationTheme AppTheme
        {
            get => Read(nameof(AppTheme), ApplicationTheme.Light);
            set => Write(nameof(AppTheme), value.ToString());
        }

        public TimeSpan CacheMaxDuration
        {
            get => Read(nameof(CacheMaxDuration), TimeSpan.FromDays(2));
            set => Write(nameof(CacheMaxDuration), value);
        }

        public string BusyText
        {
            get => Read(nameof(BusyText), "Please wait...");
            set => Write(nameof(BusyText), value);
        }

        private new void Write<T>(string key, T value)
        {
            // persist it

            base.Write(key, value);

            // implement it

            WindowEx.Current().Dispatcher.Dispatch(() =>
            {
                switch (key)
                {
                    case nameof(UseShellBackButton):
                        Template10.Services.BackButtonService.Settings.ShellBackButtonVisible = UseShellBackButton;
                        BackButtonService.GetInstance().UpdateBackButton(NavigationService.Default.CanGoBack);
                        break;
                    case nameof(AppTheme):
                        (Window.Current.Content as FrameworkElement).RequestedTheme = AppTheme.ToElementTheme();
                        break;
                    case nameof(CacheMaxDuration):
                        Template10.Navigation.Settings.CacheMaxDuration = CacheMaxDuration;
                        break;
                }
            });
        }
    }
}
