using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Template10.Services.SettingsService.Settings;

namespace Minimal.Services.SettingsServices
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SettingsService
    public class SettingsService
    {
        public static SettingsService Instance { get; private set; }

        static SettingsService()
        {
            // implement singleton pattern
            Instance = Instance ?? new SettingsService();
        }


        private SettingsService()
        {
        }

        public bool UseShellBackButton
        {
            get { return Local.Read<bool>(nameof(UseShellBackButton), true); }
            set
            {
                Local.Write(nameof(UseShellBackButton), value);
                Template10.Common.BootStrapper.Current.ShowShellBackButton = value;
                Template10.Common.BootStrapper.Current.UpdateShellBackButton();
                Template10.Common.BootStrapper.Current.NavigationService.Refresh();
            }
        }
    }
}
