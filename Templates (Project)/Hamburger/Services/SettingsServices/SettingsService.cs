using System;
using Template10.Common;
using Template10.Utils;
using Windows.UI.Xaml;

namespace Sample.Services.SettingsServices
{
    public class SettingsService : Template10.Services.SettingsService.SettingsBase
    {
        public static SettingsService Instance { get; } = new SettingsService();
        private SettingsService()
        {
            // empty
        }

        public bool UseShellBackButton
        {
            get => SettingsHelper.Read<bool>(nameof(UseShellBackButton), true);
            set
            {
                SettingsHelper.Write(nameof(UseShellBackButton), value);
                BootStrapper.Current.NavigationService.GetDispatcherWrapper().Dispatch(() =>
                {
                    BootStrapper.Current.ShowShellBackButton = value;
                    BootStrapper.Current.UpdateShellBackButton();
                });
            }
        }

        public ApplicationTheme AppTheme
        {
            get
            {
                var theme = ApplicationTheme.Light;
                var value = SettingsHelper.Read<string>(nameof(AppTheme), theme.ToString());
                return Enum.TryParse<ApplicationTheme>(value, out theme) ? theme : ApplicationTheme.Dark;
            }
            set
            {
                SettingsHelper.Write(nameof(AppTheme), value.ToString());
                (Window.Current.Content as FrameworkElement).RequestedTheme = value.ToElementTheme();
                Views.Shell.HamburgerMenu.RefreshStyles(value, true);
            }
        }

        public TimeSpan CacheMaxDuration
        {
            get => SettingsHelper.Read<TimeSpan>(nameof(CacheMaxDuration), TimeSpan.FromDays(2));
            set
            {
                SettingsHelper.Write(nameof(CacheMaxDuration), value);
                BootStrapper.Current.CacheMaxDuration = value;
            }
        }

        public bool ShowHamburgerButton
        {
            get => SettingsHelper.Read<bool>(nameof(ShowHamburgerButton), true);
            set
            {
                SettingsHelper.Write(nameof(ShowHamburgerButton), value);
                Views.Shell.HamburgerMenu.HamburgerButtonVisibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public bool IsFullScreen
        {
            get => SettingsHelper.Read<bool>(nameof(IsFullScreen), false);
            set
            {
                SettingsHelper.Write(nameof(IsFullScreen), value);
                Views.Shell.HamburgerMenu.IsFullScreen = value;
            }
        }
    }
}
