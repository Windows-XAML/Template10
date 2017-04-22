using System;
using Template10;
using Template10.Common;
using Template10.Utils;
using Windows.UI.Xaml;

namespace Sample.Services.SettingsServices
{
    public class SettingsService : Template10.Services.SettingsService.SettingsServiceBase
    {
        public static SettingsService Instance { get; } = new SettingsService();
        private SettingsService()
        {
            // empty
        }

        public bool UseShellBackButton
        {
            get => Read<bool>(nameof(UseShellBackButton), true);
            set
            {
                Write(nameof(UseShellBackButton), value);
                Locator.WindowWrapper.Current().Dispatcher.Dispatch(() =>
                {
                    Locator.BootStrapper.Instance.ShowShellBackButton = value;
                    Locator.BootStrapper.Instance.UpdateShellBackButton();
                });
            }
        }

        public ApplicationTheme AppTheme
        {
            get
            {
                var theme = ApplicationTheme.Light;
                var value = Read<string>(nameof(AppTheme), theme.ToString());
                return Enum.TryParse<ApplicationTheme>(value, out theme) ? theme : ApplicationTheme.Dark;
            }
            set
            {
                Write(nameof(AppTheme), value.ToString());
                (Window.Current.Content as FrameworkElement).RequestedTheme = value.ToElementTheme();
                Views.Shell.HamburgerMenu.RefreshStyles(value, true);
            }
        }

        public TimeSpan CacheMaxDuration
        {
            get => Read<TimeSpan>(nameof(CacheMaxDuration), TimeSpan.FromDays(2));
            set
            {
                Write(nameof(CacheMaxDuration), value);
                Locator.BootStrapper.Instance.CacheMaxDuration = value;
            }
        }

        public bool ShowHamburgerButton
        {
            get => Read<bool>(nameof(ShowHamburgerButton), true);
            set
            {
                Write(nameof(ShowHamburgerButton), value);
                Views.Shell.HamburgerMenu.HamburgerButtonVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public bool IsFullScreen
        {
            get => Read<bool>(nameof(IsFullScreen), false);
            set
            {
                Write(nameof(IsFullScreen), value);
                Views.Shell.HamburgerMenu.IsFullScreen = value;
            }
        }
    }
}
