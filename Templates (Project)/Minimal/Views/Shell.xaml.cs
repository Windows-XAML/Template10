using System;
using System.ComponentModel;
using System.Linq;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Sample.Services.SettingsServices;
using System.Collections.Generic;
using Windows.UI.Xaml.Media;

namespace Sample.Views
{
	// DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SplitView
	public sealed partial class Shell : Page
	{
		public static Shell Instance { get; set; }

		// Due to many items for HamburgerMenu to be styled (8 to be exact) 
		// a dictionary data container provides easy handling

		private static Dictionary<string, SolidColorBrush> HamburgerMenuDefaultColors;

		private static SolidColorBrush HamburgerBackgroundBrush;
		private static SolidColorBrush HamburgerForegroundBrush;
		private static SolidColorBrush NavAreaBackgroundBrush;
		private static SolidColorBrush NavButtonBackgroundBrush;

		private static SolidColorBrush NavButtonForegroundBrush;
		private static SolidColorBrush SecondarySeparatorBrush;
		private static SolidColorBrush NavButtonCheckedBackgroundBrush;
		private static SolidColorBrush NavButtonCheckedForegroundBrush;
		private static SolidColorBrush NavButtonHoverBackgroundBrush;

		public Shell(NavigationService navigationService)
		{
			Instance = this;
			InitializeComponent();
			MyHamburgerMenu.NavigationService = navigationService;
			VisualStateManager.GoToState(Instance, Instance.NormalVisualState.Name, true);
			PopulateDefaultColors();
			ElementTheme startupTheme = (SettingsService.Instance.AppTheme == ApplicationTheme.Light)? ElementTheme.Light : ElementTheme.Default;
			SetThemeColors(startupTheme);
		}

		public static void SetBusyVisibility(Visibility visible, string text = null)
		{
			WindowWrapper.Current().Dispatcher.Dispatch(() =>
			{
				switch (visible)
				{
					case Visibility.Visible:
						Instance.FindName(nameof(BusyScreen));
						Instance.BusyText.Text = text ?? string.Empty;
						if (VisualStateManager.GoToState(Instance, Instance.BusyVisualState.Name, true))
						{
							SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
								AppViewBackButtonVisibility.Collapsed;
						}
						break;
					case Visibility.Collapsed:
						if (VisualStateManager.GoToState(Instance, Instance.NormalVisualState.Name, true))
						{
							BootStrapper.Current.UpdateShellBackButton();
						}
						break;
				}
			});
		}

		public static void SetThemeColors(ElementTheme theme)
		{

			WindowWrapper.Current().Dispatcher.Dispatch(() =>
			{

				Instance.RequestedTheme = theme;
				ParseStyleforThemes(theme);

				Instance.MyHamburgerMenu.HamburgerBackground = HamburgerBackgroundBrush;
				Instance.MyHamburgerMenu.HamburgerForeground = HamburgerForegroundBrush;
				Instance.MyHamburgerMenu.NavAreaBackground = NavAreaBackgroundBrush;
				Instance.MyHamburgerMenu.NavButtonBackground = NavButtonBackgroundBrush;
				Instance.MyHamburgerMenu.NavButtonForeground = NavButtonForegroundBrush;
				Instance.MyHamburgerMenu.SecondarySeparator = SecondarySeparatorBrush;
				Instance.MyHamburgerMenu.NavButtonCheckedBackground = NavButtonCheckedBackgroundBrush;
				Instance.MyHamburgerMenu.NavButtonCheckedForeground = NavButtonCheckedForegroundBrush;
				Instance.MyHamburgerMenu.NavButtonHoverBackground = NavButtonHoverBackgroundBrush;

				BootStrapper.Current.NavigationService.Refresh();
			});
		}

		private static void ParseStyleforThemes(ElementTheme theme)
		{
			string ThemeColor = (theme == ElementTheme.Light) ? nameof(ElementTheme.Light) : nameof(ElementTheme.Default);

			try
			{
				var myResourceDictionary = new ResourceDictionary();
				myResourceDictionary.Source = new Uri("ms-appx:///Styles/Custom.xaml", UriKind.RelativeOrAbsolute);
				ResourceDictionary themeResource = myResourceDictionary.ThemeDictionaries[ThemeColor] as ResourceDictionary;

				Style HamburgerMenuStyle = themeResource[typeof(HamburgerMenu)] as Style;

				HamburgerBackgroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.HamburgerBackgroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
				HamburgerForegroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.HamburgerForegroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
				NavAreaBackgroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavAreaBackgroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
				NavButtonBackgroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavButtonBackgroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;

				NavButtonForegroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavButtonForegroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
				SecondarySeparatorBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.SecondarySeparatorProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
				NavButtonCheckedBackgroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavButtonCheckedBackgroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
				NavButtonCheckedForegroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavButtonCheckedForegroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
				NavButtonHoverBackgroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavButtonHoverBackgroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
			}
			catch
			{
				// defaults picked up in PopulateDefaultColors() will take care of this!
			}

			// We are defensive against missing style colors (ie, null values) just in case the Custom.xaml style file is messed up,
			// so we pick up hard-wired defaults if a value is missing.

			HamburgerBackgroundBrush = HamburgerBackgroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(HamburgerBackgroundBrush)];
			HamburgerForegroundBrush = HamburgerForegroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(HamburgerForegroundBrush)];
			NavAreaBackgroundBrush = NavAreaBackgroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavAreaBackgroundBrush)];
			NavButtonBackgroundBrush = NavButtonBackgroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonBackgroundBrush)];

			NavButtonForegroundBrush = NavButtonForegroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonForegroundBrush)];
			SecondarySeparatorBrush = SecondarySeparatorBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(SecondarySeparatorBrush)];
			NavButtonCheckedBackgroundBrush = NavButtonCheckedBackgroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedBackgroundBrush)];
			NavButtonCheckedForegroundBrush = NavButtonCheckedForegroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedForegroundBrush)];
			NavButtonHoverBackgroundBrush = NavButtonHoverBackgroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonHoverBackgroundBrush)];
		}

		private void PopulateDefaultColors()
		{
			HamburgerMenuDefaultColors = new Dictionary<string, SolidColorBrush>();

			string ThemeColor = nameof(ElementTheme.Light);

			HamburgerMenuDefaultColors[ThemeColor + nameof(HamburgerBackgroundBrush)] = new SolidColorBrush(Colors.Gainsboro);
			HamburgerMenuDefaultColors[ThemeColor + nameof(HamburgerForegroundBrush)] = new SolidColorBrush(Colors.Black);
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavAreaBackgroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0xf2, 0xf2, 0xf2));
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonBackgroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0xf2, 0xf2, 0xf2));

			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonForegroundBrush)] = new SolidColorBrush(Colors.Black);
			HamburgerMenuDefaultColors[ThemeColor + nameof(SecondarySeparatorBrush)] = new SolidColorBrush(Colors.Gray);
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedBackgroundBrush)] = new SolidColorBrush(Colors.PowderBlue);
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedForegroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0xcc, 0xe3, 0xf5));
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonHoverBackgroundBrush)] = new SolidColorBrush(Colors.LightSteelBlue);

			ThemeColor = nameof(ElementTheme.Default);

			HamburgerMenuDefaultColors[ThemeColor + nameof(HamburgerBackgroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0xd1, 0x34, 0x38));
			HamburgerMenuDefaultColors[ThemeColor + nameof(HamburgerForegroundBrush)] = new SolidColorBrush(Colors.White);
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavAreaBackgroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0x2b, 0x2b, 0x2b));
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonBackgroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0x2b, 0x2b, 0x2b));

			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonForegroundBrush)] = new SolidColorBrush(Colors.White);
			HamburgerMenuDefaultColors[ThemeColor + nameof(SecondarySeparatorBrush)] = new SolidColorBrush(Colors.Gray);
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedBackgroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0x7d, 0x1f, 0x22));
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedForegroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0x43, 0x43, 0x43));
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonHoverBackgroundBrush)] = new SolidColorBrush(Colors.SlateGray);

		}
	}
}
