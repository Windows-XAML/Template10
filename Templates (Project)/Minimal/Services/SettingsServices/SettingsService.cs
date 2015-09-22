using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Sample.Services.SettingsServices
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SettingsService
    public class SettingsService : ISettingsService
    {
        public static SettingsService Instance { get; private set; }

        static SettingsService()
        {
            // implement singleton pattern
            Instance = Instance ?? new SettingsService();
        }

        Template10.Services.SettingsService.ISettingsHelper _helper;

        private SettingsService()
        {
            _helper = new Template10.Services.SettingsService.SettingsHelper();
        }

        public bool UseShellBackButton
        {
            get { return _helper.Read<bool>(nameof(UseShellBackButton), true); }
            set
            {
                _helper.Write(nameof(UseShellBackButton), value);

                // implement
                Template10.Common.BootStrapper.Current.NavigationService.Dispatcher.Dispatch(() =>
                {
                    Template10.Common.BootStrapper.Current.ShowShellBackButton = value;
                    Template10.Common.BootStrapper.Current.UpdateShellBackButton();
                    Template10.Common.BootStrapper.Current.NavigationService.Refresh();
                });
            }
        }

        public ApplicationTheme AppTheme
        {
            get
            {
                var theme = ApplicationTheme.Light;
                var value = _helper.Read<string>(nameof(AppTheme), theme.ToString());
                Enum.TryParse<ApplicationTheme>(value, out theme);
                return theme;
            }
            set
            {
                _helper.Write(nameof(AppTheme), value.ToString());

                // implement
                Template10.Common.BootStrapper.Current.NavigationService.Dispatcher.Dispatch(() =>
                {
                    switch (value)
                    {
                        case ApplicationTheme.Light:
                            Views.Shell.Instance.RequestedTheme = ElementTheme.Light;
                            break;
                        case ApplicationTheme.Dark:
                            Views.Shell.Instance.RequestedTheme = ElementTheme.Dark;
                            break;
                        default:
                            Views.Shell.Instance.RequestedTheme = ElementTheme.Default;
                            break;
                    }
                    Template10.Common.BootStrapper.Current.NavigationService.Refresh();
                });
            }
        }

        public TimeSpan CacheMaxDuration
        {
            get { return _helper.Read<TimeSpan>(nameof(CacheMaxDuration), TimeSpan.FromDays(2)); }
            set { _helper.Write(nameof(CacheMaxDuration), value); }
        }
    }
}
