using System;
using Template10.Common;
using Template10.Utils;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Sample.Services.SettingsServices
{
    public class SettingsService
    {
        public static SettingsService Instance { get; } = new SettingsService();
        Template10.Services.SettingsService.ISettingsHelper _helper;
        private SettingsService()
        {
            _helper = new Template10.Services.SettingsService.SettingsHelper();
        }

        public ApplicationTheme AppTheme
        {
            get
            {
                var theme = ApplicationTheme.Dark;
                var value = _helper.Read<string>(nameof(AppTheme), theme.ToString());
                return Enum.TryParse<ApplicationTheme>(value, out theme) ? theme : ApplicationTheme.Dark;
            }
            set
            {
                _helper.Write(nameof(AppTheme), value.ToString());
                (Window.Current.Content as FrameworkElement).RequestedTheme = value.ToElementTheme();
                Views.Shell.SetRequestedTheme(value, UseBackgroundChecked);
            }
        }

         public SplitViewDisplayMode CompactDisplayMode
        {
            get
            {
                var mode = SplitViewDisplayMode.CompactOverlay;
                var value = _helper.Read<string>(nameof(CompactDisplayMode), SplitViewDisplayMode.CompactOverlay.ToString());
                return Enum.TryParse<SplitViewDisplayMode>(value, out mode) ? mode : SplitViewDisplayMode.CompactInline;
            }
            set
            {
                _helper.Write(nameof(CompactDisplayMode), value.ToString());
                Views.Shell.HamburgerMenu.DisplayMode = value;
                Views.Shell.HamburgerMenu.VisualStateNormalDisplayMode = Views.Shell.HamburgerMenu.VisualStateWideDisplayMode = value;

                if (value == SplitViewDisplayMode.CompactOverlay)
                {
                    Views.Shell.HamburgerMenu.IsOpen = false;}
                else
                {
                    Views.Shell.HamburgerMenu.IsOpen = true;
                }
            }
        }

        public bool UseBackgroundChecked
        {
            get { return _helper.Read<bool>(nameof(UseBackgroundChecked), true); }
            set
            {
                _helper.Write(nameof(UseBackgroundChecked), value);
                Views.Shell.SetRequestedTheme(AppTheme, value);
            }
        }
        

        public TimeSpan CacheMaxDuration
        {
            get { return _helper.Read<TimeSpan>(nameof(CacheMaxDuration), TimeSpan.FromDays(2)); }
            set
            {
                _helper.Write(nameof(CacheMaxDuration), value);
                BootStrapper.Current.CacheMaxDuration = value;
            }
        }
    }
}
