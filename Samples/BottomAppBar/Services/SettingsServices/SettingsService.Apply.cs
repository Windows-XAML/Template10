using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Sample.Services.SettingsServices
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

