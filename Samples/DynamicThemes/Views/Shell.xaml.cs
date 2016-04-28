using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Sample.Services.SettingsServices;
using Sample.Utils;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.ApplicationModel.Resources;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Sample.Views
{
	public sealed partial class Shell : Page, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public static Shell Instance { get; set; }
		public static HamburgerMenu HamburgerMenu => Instance.MyHamburgerMenu;

		#region custom fields

		SettingsService settings;
		ResourceLoader strings;
		private static Dictionary<string, SolidColorBrush> HamburgerMenuDefaultColors;

		private static SolidColorBrush HamburgerForegroundBrush;
		private static SolidColorBrush HamburgerBackgroundBrush;

		private static SolidColorBrush NavButtonForegroundBrush;
		private static SolidColorBrush NavButtonBackgroundBrush;

		private static SolidColorBrush NavButtonCheckedForegroundBrush;
		private static SolidColorBrush NavButtonCheckedBackgroundBrush;

		private static SolidColorBrush NavButtonPressedForegroundBrush; //**
		private static SolidColorBrush NavButtonPressedBackgroundBrush;

		private static SolidColorBrush NavButtonHoverForegroundBrush;
		private static SolidColorBrush NavButtonHoverBackgroundBrush;

		private static SolidColorBrush NavAreaBackgroundBrush;
		private static SolidColorBrush NavButtonCheckedIndicatorBrush; //**
		private static SolidColorBrush SecondarySeparatorBrush;

		#endregion

		public Shell()
		{
			Instance = this;
			InitializeComponent();
		}

		public Shell(INavigationService navigationService) : this()
		{
			SetNavigationService(navigationService);
			settings = SettingsService.Instance;
			//strings = ResourceLoader.GetForViewIndependentUse(); // STRANGE! This makes SplashScreen to get stuck, but why?

			PopulateDefaultColors();
			SetRequestedTheme(settings.AppTheme);
			SecondaryButtonsMaxWidth = ((HamburgerMenu.SecondaryButtonOrientation == Orientation.Horizontal)) ? 48 : HamburgerMenu.PaneWidth;
		}

		public void SetNavigationService(INavigationService navigationService)
		{
			MyHamburgerMenu.NavigationService = navigationService;
		}

		public static void SetRequestedTheme(ApplicationTheme theme)
		{

			WindowWrapper.Current().Dispatcher.Dispatch(() =>
			{
				(Window.Current.Content as FrameworkElement).RequestedTheme = theme.ToElementTheme();

				ParseStyleforThemes(theme);

				HamburgerMenu.HamburgerForeground = HamburgerForegroundBrush;
				HamburgerMenu.HamburgerBackground = HamburgerBackgroundBrush;

				HamburgerMenu.NavButtonForeground = NavButtonForegroundBrush;
				HamburgerMenu.NavButtonBackground = NavButtonBackgroundBrush;

				HamburgerMenu.NavButtonCheckedForeground = NavButtonCheckedForegroundBrush;
				HamburgerMenu.NavButtonCheckedBackground = NavButtonCheckedBackgroundBrush;

				HamburgerMenu.NavButtonPressedForeground = NavButtonPressedForegroundBrush;
				HamburgerMenu.NavButtonPressedBackground = NavButtonPressedBackgroundBrush;

				HamburgerMenu.NavButtonHoverForeground = NavButtonHoverForegroundBrush;
				HamburgerMenu.NavButtonHoverBackground = NavButtonHoverBackgroundBrush;

				HamburgerMenu.NavAreaBackground = NavAreaBackgroundBrush;
				HamburgerMenu.NavButtonCheckedIndicatorBrush = NavButtonCheckedIndicatorBrush;
				HamburgerMenu.SecondarySeparator = SecondarySeparatorBrush;

				// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
				// Reason for proposal for PR #884 (Public Property wrapper for readonly LoadedNavButtons)

				List<HamburgerButtonInfo> NavButtons = HamburgerMenu.PrimaryButtons.ToList();
				NavButtons.InsertRange(NavButtons.Count, HamburgerMenu.SecondaryButtons.ToList());

				foreach(var hbi in NavButtons)
				{
					if (hbi.ButtonType != HamburgerButtonInfo.ButtonTypes.Toggle) continue;

					if (!hbi.IsChecked ?? false) continue;
					StackPanel sp = hbi.Content as StackPanel;
					ToggleButton btn = sp.Parent as ToggleButton;

					ContentPresenter cp = VisualTree.GetFirstAncestorOfType<ContentPresenter>(btn);
					cp.Background = NavButtonCheckedBackgroundBrush;
					cp.Foreground = NavButtonCheckedForegroundBrush;
					Rectangle indicator = VisualTree.GetFirstDescendantOfType<Rectangle>(btn);
					indicator.Visibility = Visibility.Visible;
				}

				// >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

			});
		}

		private static void ParseStyleforThemes(ApplicationTheme theme)
		{
			string ThemeColor = (theme == ApplicationTheme.Light) ? nameof(ElementTheme.Light) : nameof(ElementTheme.Default);

			try
			{
				var myResourceDictionary = new ResourceDictionary();
				myResourceDictionary.Source = new Uri("ms-appx:///Styles/Custom.xaml", UriKind.RelativeOrAbsolute);
				ResourceDictionary themeResource = myResourceDictionary.ThemeDictionaries[ThemeColor] as ResourceDictionary;

				Style HamburgerMenuStyle = themeResource[typeof(HamburgerMenu)] as Style;

				HamburgerForegroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.HamburgerForegroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
				HamburgerBackgroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.HamburgerBackgroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;

				NavButtonForegroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavButtonForegroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
				NavButtonBackgroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavButtonBackgroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;

				NavButtonCheckedForegroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavButtonCheckedForegroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
				NavButtonCheckedBackgroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavButtonCheckedBackgroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;

				NavButtonPressedForegroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavButtonPressedForegroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
				NavButtonPressedBackgroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavButtonPressedBackgroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;

				NavButtonHoverForegroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavButtonHoverForegroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
				NavButtonHoverBackgroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavButtonHoverBackgroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;

				NavAreaBackgroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavAreaBackgroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
				NavButtonCheckedIndicatorBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavButtonCheckedIndicatorBrushProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
				SecondarySeparatorBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.SecondarySeparatorProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;

			}
			catch
			{
				// defaults below will take care of this!
			}

			// We are defensive against missing style colors (ie, null values) just in case the Custom.xaml style file is messed up,
			// so we pick up defaults if a value is missing.

			HamburgerForegroundBrush = HamburgerForegroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(HamburgerForegroundBrush)];
			HamburgerBackgroundBrush = HamburgerBackgroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(HamburgerBackgroundBrush)];

			NavButtonForegroundBrush = NavButtonForegroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonForegroundBrush)];
			NavButtonBackgroundBrush = NavButtonBackgroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonBackgroundBrush)];

			NavButtonCheckedForegroundBrush = NavButtonCheckedForegroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedForegroundBrush)];
			NavButtonCheckedBackgroundBrush = NavButtonCheckedBackgroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedBackgroundBrush)];

			NavButtonPressedForegroundBrush = NavButtonPressedForegroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonPressedForegroundBrush)];
			NavButtonPressedBackgroundBrush = NavButtonPressedBackgroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonPressedBackgroundBrush)];

			NavButtonHoverForegroundBrush = NavButtonHoverForegroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonHoverForegroundBrush)];
			NavButtonHoverBackgroundBrush = NavButtonHoverBackgroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonHoverBackgroundBrush)];

			NavAreaBackgroundBrush = NavAreaBackgroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavAreaBackgroundBrush)];
			NavButtonCheckedIndicatorBrush = NavButtonCheckedIndicatorBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedIndicatorBrush)];
			SecondarySeparatorBrush = SecondarySeparatorBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(SecondarySeparatorBrush)];
		}

		private void PopulateDefaultColors()
		{
			HamburgerMenuDefaultColors = new Dictionary<string, SolidColorBrush>();

			string ThemeColor = nameof(ElementTheme.Light);

			HamburgerMenuDefaultColors[ThemeColor + nameof(HamburgerForegroundBrush)] = new SolidColorBrush(Colors.Black);
			HamburgerMenuDefaultColors[ThemeColor + nameof(HamburgerBackgroundBrush)] = new SolidColorBrush(Colors.Gainsboro);

			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonForegroundBrush)] = new SolidColorBrush(Colors.Black);
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonBackgroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0xf2, 0xf2, 0xf2));

			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedForegroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0xcc, 0xe3, 0xf5));
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedBackgroundBrush)] = new SolidColorBrush(Colors.PowderBlue);

			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonPressedForegroundBrush)] = new SolidColorBrush(Colors.White);
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonPressedBackgroundBrush)] = new SolidColorBrush(Colors.LightBlue);

			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonHoverForegroundBrush)] = new SolidColorBrush(Colors.White);
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonHoverBackgroundBrush)] = new SolidColorBrush(Colors.LightSteelBlue);

			HamburgerMenuDefaultColors[ThemeColor + nameof(NavAreaBackgroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0xf2, 0xf2, 0xf2));
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedIndicatorBrush)] = (SolidColorBrush)Application.Current.Resources["MenuIndicatorLightBrush"];
			HamburgerMenuDefaultColors[ThemeColor + nameof(SecondarySeparatorBrush)] = new SolidColorBrush(Colors.Gray);

			ThemeColor = nameof(ElementTheme.Default);

			HamburgerMenuDefaultColors[ThemeColor + nameof(HamburgerForegroundBrush)] = new SolidColorBrush(Colors.White);
			HamburgerMenuDefaultColors[ThemeColor + nameof(HamburgerBackgroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0xd1, 0x34, 0x38));

			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonForegroundBrush)] = new SolidColorBrush(Colors.White);
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonBackgroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0x2b, 0x2b, 0x2b));

			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedForegroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0x43, 0x43, 0x43));
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedBackgroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0x7d, 0x1f, 0x22));

			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonPressedForegroundBrush)] = new SolidColorBrush(Colors.White);
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonPressedBackgroundBrush)] = new SolidColorBrush(Colors.DarkSlateGray);

			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonHoverForegroundBrush)] = new SolidColorBrush(Colors.White);
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonHoverBackgroundBrush)] = new SolidColorBrush(Colors.SlateGray);

			HamburgerMenuDefaultColors[ThemeColor + nameof(NavAreaBackgroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0x2b, 0x2b, 0x2b));
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedIndicatorBrush)] = (SolidColorBrush)Application.Current.Resources["MenuIndicatorDarkBrush"];
			HamburgerMenuDefaultColors[ThemeColor + nameof(SecondarySeparatorBrush)] = new SolidColorBrush(Colors.Gray);

		}

		private double secondaryButtonsMaxWidth;
		public double SecondaryButtonsMaxWidth
		{
			get
			{
				return secondaryButtonsMaxWidth;
			}

			set
			{
				secondaryButtonsMaxWidth = value;
				Instance.PropertyChanged?.Invoke(Instance, new PropertyChangedEventArgs(nameof(SecondaryButtonsMaxWidth)));
			}
		}
	}
}
