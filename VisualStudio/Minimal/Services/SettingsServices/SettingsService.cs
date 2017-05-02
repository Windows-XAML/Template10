using System;
using Template10.Common;
using Template10.Services.WindowWrapper;
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
            get { return Read<bool>(nameof(UseShellBackButton), true); }
            set
            {
                Write(nameof(UseShellBackButton), value);
				WindowWrapper.Current().Dispatcher.Dispatch(() =>
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
                var value = Read<string>(nameof(AppTheme), theme.ToString());
                return Enum.TryParse<ApplicationTheme>(value, out theme) ? theme : ApplicationTheme.Dark;
            }
            set
            {
                Write(nameof(AppTheme), value.ToString());
                (Window.Current.Content as FrameworkElement).RequestedTheme = value.ToElementTheme();
            }
        }

        public TimeSpan CacheMaxDuration
        {
            get { return Read<TimeSpan>(nameof(CacheMaxDuration), TimeSpan.FromDays(2)); }
            set
            {
                Write(nameof(CacheMaxDuration), value);
                BootStrapper.Current.CacheMaxDuration = value;
            }
        }
    }
}
