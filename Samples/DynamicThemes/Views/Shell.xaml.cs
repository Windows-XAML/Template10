using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Sample.Services.SettingsServices;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Template10.Utils;
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
		private static Dictionary<string, SolidColorBrush> HamburgerMenuDefaultColors;

		private static SolidColorBrush NavButtonCheckedForegroundBrush;
		private static SolidColorBrush NavButtonCheckedBackgroundBrush;
		private static SolidColorBrush NavButtonCheckedIndicatorBrush;
		private static SolidColorBrush NavButtonBackgroundBrush;
		private static SolidColorBrush SecondarySeparatorBrush;

		#endregion

		public Shell()
		{
			Instance = this;
			InitializeComponent();
			Loaded += Shell_Loaded;
		}

		public Shell(INavigationService navigationService) : this()
		{
			SetNavigationService(navigationService);
			settings = SettingsService.Instance;
		}

		private void Shell_Loaded(object sender, RoutedEventArgs e)
		{
			PopulateDefaultColors();
			SetRequestedTheme(settings.AppTheme, settings.UseBackgroundChecked);

			SecondaryButtonsMaxWidth = ((HamburgerMenu.SecondaryButtonOrientation == Orientation.Horizontal)) ? 48 : HamburgerMenu.PaneWidth;
			HamburgerMenu.DisplayMode = settings.CompactDisplayMode;
			HamburgerMenu.VisualStateNormalDisplayMode = HamburgerMenu.VisualStateWideDisplayMode = settings.CompactDisplayMode;
		}

		public void SetNavigationService(INavigationService navigationService)
		{
			MyHamburgerMenu.NavigationService = navigationService;
		}

		public static void SetRequestedTheme(ApplicationTheme theme, bool UseBackgroundChecked)
		{

			WindowWrapper.Current().Dispatcher.Dispatch(() =>
			{
				(Window.Current.Content as FrameworkElement).RequestedTheme = theme.ToElementTheme();

				ParseStyleforThemes(theme);

				HamburgerMenu.NavButtonCheckedForeground = NavButtonCheckedForegroundBrush;

				HamburgerMenu.NavButtonCheckedBackground = (UseBackgroundChecked) ?
					NavButtonCheckedBackgroundBrush : NavButtonBackgroundBrush;

				HamburgerMenu.NavButtonCheckedIndicatorBrush = (UseBackgroundChecked) ?
					Colors.Transparent.ToSolidColorBrush() : NavButtonCheckedIndicatorBrush;

				HamburgerMenu.SecondarySeparator = SecondarySeparatorBrush;

				List<HamburgerButtonInfo> NavButtons = HamburgerMenu.PrimaryButtons.ToList();
				NavButtons.InsertRange(NavButtons.Count, HamburgerMenu.SecondaryButtons.ToList());

				List<HamburgerMenu.InfoElement> LoadedNavButtons = new List<HamburgerMenu.InfoElement>();

				foreach (var hbi in NavButtons)
				{
					StackPanel sp = hbi.Content as StackPanel;
					if (hbi.ButtonType == HamburgerButtonInfo.ButtonTypes.Literal) continue;
					ToggleButton tBtn = sp.Parent as ToggleButton;
					Button btn = sp.Parent as Button;

					if (tBtn != null)
					{
						var button = new HamburgerMenu.InfoElement(tBtn);
						LoadedNavButtons.Add(button);
					}
					else if (btn != null)
					{
						var button = new HamburgerMenu.InfoElement(btn);
						LoadedNavButtons.Add(button);
						continue;
					}
					else
					{
						continue;
					}

					Rectangle indicator = tBtn.FirstChild<Rectangle>();
					indicator.Visibility = ((!hbi.IsChecked ?? false)) ? Visibility.Collapsed : Visibility.Visible;

					if (!hbi.IsChecked ?? false) continue;

					ContentPresenter cp = tBtn.FirstAncestor<ContentPresenter>();
					cp.Background = NavButtonCheckedBackgroundBrush;
					cp.Foreground = NavButtonCheckedForegroundBrush;
				}

				LoadedNavButtons.ForEach(x => x.RefreshVisualState());

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

				Style HamburgerMenuStyle = themeResource["HamburgerMenuStyle"] as Style;

				NavButtonCheckedForegroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavButtonCheckedForegroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
				NavButtonCheckedBackgroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavButtonCheckedBackgroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
				NavButtonCheckedIndicatorBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavButtonCheckedIndicatorBrushProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
				NavButtonBackgroundBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.NavButtonBackgroundProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
				SecondarySeparatorBrush = ((from item in HamburgerMenuStyle.Setters.Cast<Setter>().Where(item => item.Property == HamburgerMenu.SecondarySeparatorProperty) select item).SingleOrDefault())?.Value as SolidColorBrush;
			}

			catch
			{
				// PopulateDefaultColors() will take care of this!
			}

			NavButtonCheckedForegroundBrush = NavButtonCheckedForegroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedForegroundBrush)];
			NavButtonCheckedBackgroundBrush = NavButtonCheckedBackgroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedBackgroundBrush)];
			NavButtonCheckedIndicatorBrush = NavButtonCheckedIndicatorBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedIndicatorBrush)];
			NavButtonBackgroundBrush = NavButtonBackgroundBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonBackgroundBrush)];
			SecondarySeparatorBrush = SecondarySeparatorBrush ?? HamburgerMenuDefaultColors[ThemeColor + nameof(SecondarySeparatorBrush)];
		}

		private void PopulateDefaultColors()
		{
			HamburgerMenuDefaultColors = new Dictionary<string, SolidColorBrush>();

			string ThemeColor = nameof(ElementTheme.Light);

			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedForegroundBrush)] = new SolidColorBrush(Colors.Black);
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedBackgroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0x42, 0xb7, 0xff));
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedIndicatorBrush)] = (SolidColorBrush)Application.Current.Resources["MenuIndicatorLightBrush"];
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonBackgroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0xf2, 0xf2, 0xf2));
				
			HamburgerMenuDefaultColors[ThemeColor + nameof(SecondarySeparatorBrush)] = new SolidColorBrush(Colors.Gray);

			ThemeColor = nameof(ElementTheme.Default);

			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedForegroundBrush)] = new SolidColorBrush(Colors.White);
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedBackgroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0x2a, 0x4e, 0x6c));
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonCheckedIndicatorBrush)] = (SolidColorBrush)Application.Current.Resources["MenuIndicatorDarkBrush"];
			HamburgerMenuDefaultColors[ThemeColor + nameof(NavButtonBackgroundBrush)] = new SolidColorBrush(Color.FromArgb(0xff, 0x2b, 0x2b, 0x2b));
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
