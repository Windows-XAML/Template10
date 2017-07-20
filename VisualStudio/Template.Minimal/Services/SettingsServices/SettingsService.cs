using System;
using Template10.Services.NavigationService;
using Template10.Services.SettingsService.Services.SettingsService;
using Template10.Services.WindowWrapper;
using Template10.Utils;
using Windows.UI.Xaml;

namespace Sample.Services.SettingsServices
{
    public class SettingsService : SettingsServiceBase
    {
        public static SettingsService Instance;
        static SettingsService()
        {
            Instance = new SettingsService();
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

        private new void Write<T>(string key, T value)
        {
            // persist it

            base.Write(key, value);

            // implement it

            WindowWrapperHelper.Current().Dispatcher.Dispatch(() =>
            {
                switch (key)
                {
                    case nameof(UseShellBackButton):
                        Template10.Services.BackButtonService.Settings.ShellBackButtonVisible = UseShellBackButton;
                        Template10.Services.BackButtonService.BackButtonService.Instance.UpdateBackButton(NavigationServiceHelper.Default.CanGoBack);
                        break;
                    case nameof(AppTheme):
                        (Window.Current.Content as FrameworkElement).RequestedTheme = AppTheme.ToElementTheme();
                        break;
                    case nameof(CacheMaxDuration):
                        Template10.Common.Settings.CacheMaxDuration = CacheMaxDuration;
                        break;
                }
            });
        }
    }
}
