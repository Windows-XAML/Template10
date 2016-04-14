using System;
using Windows.UI.Xaml;

namespace Template10.Samples.VoiceAndInkSample.Services.SettingsServices
{
    public partial class SettingsService
    {
        public void ApplyUseShellBackButton(bool value)
        {
            Template10.Common.BootStrapper.Current.NavigationService.Dispatcher.Dispatch(() =>
            {
                Template10.Common.BootStrapper.Current.ShowShellBackButton = value;
                Template10.Common.BootStrapper.Current.UpdateShellBackButton();
                Template10.Common.BootStrapper.Current.NavigationService.Refresh();
            });
        }

        public void ApplyAppTheme(ApplicationTheme value)
        {
            Views.Shell.HamburgerMenu.RefreshStyles(value);
        }

        private void ApplyCacheMaxDuration(TimeSpan value)
        {
            Template10.Common.BootStrapper.Current.CacheMaxDuration = value;
        }
    }
}

