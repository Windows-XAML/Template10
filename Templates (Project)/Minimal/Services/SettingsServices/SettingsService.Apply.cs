using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Template10.Common;

namespace Sample.Services.SettingsServices
{
    public partial class SettingsService
    {
        public void ApplyUseShellBackButton(bool value)
        {
            BootStrapper.Current.NavigationService.Dispatcher.Dispatch(() =>
            {
                BootStrapper.Current.ShowShellBackButton = value;
                BootStrapper.Current.UpdateShellBackButton();
                BootStrapper.Current.NavigationService.Refresh();
            });
        }

        public void ApplyAppTheme(ApplicationTheme value)
        {
            BootStrapper.Current.HamburgerMenu.RefreshStyles(value);
            Views.Shell.SetRequestedTheme(value);
        }

        private void ApplyCacheMaxDuration(TimeSpan value)
        {
            BootStrapper.Current.CacheMaxDuration = value;
        }
    }
}
