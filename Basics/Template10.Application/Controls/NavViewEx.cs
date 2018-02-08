using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Windows.Services;
using Prism.Windows.Navigation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Prism.Windows.Mvvm;
using Prism.Ioc;
using System.Collections.ObjectModel;
using Prism.Windows.Utilities;
using Windows.UI.ViewManagement;

namespace Prism.Windows.Controls
{
    public class NavViewEx : NavigationView
    {
        private static IPageRegistry _registry;

        static NavViewEx()
        {
            _registry = PrismApplicationBase.Container.Resolve<IPageRegistry>();
        }

        public IPlatformNavigationService NavigationService { get; }

        private CoreDispatcher _dispatcher;

        private Frame _frame;

        public NavViewEx()
        {
            DefaultStyleKey = typeof(NavigationView);

            Content = _frame = new Frame();
            _dispatcher = _frame.Dispatcher;

            _frame.Navigated += (s, e) =>
            {
                if (TryFindItem(e.SourcePageType, out var item))
                {
                    SetSelectedItem(item);
                }
            };

            NavigationService = new NavigationService(_frame);
            NavigationService.CanGoBackChanged += (s, e) =>
            {
                UpdateBackButton();
            };

            ItemInvoked += (s, e) =>
            {
                SelectedItem = (e.IsSettingsInvoked) ? SettingsItem : Find(e.InvokedItem.ToString());
            };
            RegisterPropertyChangedCallback(IsPaneOpenProperty, (s, e) => UpdatePaneHeaders());

            Window.Current.CoreWindow.SizeChanged += (s, e) =>
            {
                UpdatePageHeader();
            };

            Loaded += (s, e) =>
            {
                UpdatePaneHeaders();
                UpdateBackButton();
                UpdatePageHeader();
                UpdatePageCommands();
            };
        }

        private void UpdatePageCommands()
        {
            //if (_frame.Content is Page page)
            //{
            //    var value = page.GetValue(NavViewProps.HeaderCommandsProperty);
            //    if (value is ObservableCollection<object> list && (list?.Any() ?? false))
            //    {
            //        var bar = XamlUtilities.RecurseChildren(this).OfType<CommandBar>().Single();
            //        bar.PrimaryCommands.Clear();
            //        foreach (var item in list.OfType<ICommandBarElement>())
            //        {
            //            bar.PrimaryCommands.Add(item);
            //        }
            //    }
            //}
        }

        public IPlatformNavigationService Start()
        {
            var first = MenuItems
                .OfType<NavigationViewItem>()
                .SingleOrDefault(x => (bool)x.GetValue(NavViewProps.IsStartPageProperty));

            if (first != null)
            {
                SetSelectedItem(first);
            }

            return NavigationService;
        }

        public enum BackButtonVisibilities { Auto, Visible, Collapsed }
        public BackButtonVisibilities BackButtonVisibility { get; set; } = BackButtonVisibilities.Auto;

        public enum ItemHeaderBehaviors { Hide, Remove, None }
        public ItemHeaderBehaviors ItemHeaderBehavior { get; set; } = ItemHeaderBehaviors.Remove;

        public enum PageHeaderBehaviors { Adjacent, Overlay, Collapsed }
        public PageHeaderBehaviors PageHeaderBehavior { get; set; } = PageHeaderBehaviors.Adjacent;

        public Uri SettingsNavigationUri { get; set; }
        public event EventHandler SettingsInvoked;

        public new object SelectedItem
        {
            set => SetSelectedItem(value);
            get => base.SelectedItem;
        }

        private async void SetSelectedItem(object selectedItem)
        {
            if (selectedItem == null)
            {
                base.SelectedItem = null;
            }
            else if (selectedItem == base.SelectedItem)
            {
                // already set
            }
            else if (selectedItem == SettingsItem)
            {
                if (SettingsNavigationUri != null)
                {
                    await NavigationService.NavigateAsync(SettingsNavigationUri);
                    base.SelectedItem = selectedItem;
                }
                SettingsInvoked?.Invoke(this, EventArgs.Empty);
            }
            else if (selectedItem is NavigationViewItem item)
            {
                if (item.GetValue(NavViewProps.NavigationUriProperty) is string path)
                {
                    if ((await NavigationService.NavigateAsync(path)).Success)
                    {
                        base.SelectedItem = selectedItem;
                    }
                    else
                    {
                        Debug.WriteLine($"{selectedItem}.{nameof(NavViewProps.NavigationUriProperty)} navigation failed.");
                    }
                }
                else
                {
                    Debug.WriteLine($"{selectedItem}.{nameof(NavViewProps.NavigationUriProperty)} is not valid Uri");
                }
            }
            UpdatePaneHeaders();
            UpdateBackButton();
            UpdatePageHeader();
            UpdatePageCommands();
        }

        private void UpdateBackButton()
        {
            switch (BackButtonVisibility)
            {
                case BackButtonVisibilities.Auto:
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                            (_frame.CanGoBack)
                            ? AppViewBackButtonVisibility.Visible
                            : AppViewBackButtonVisibility.Collapsed;
                    break;
                case BackButtonVisibilities.Visible:
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                    break;
                case BackButtonVisibilities.Collapsed:
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    break;
            }
        }

        private void UpdatePaneHeaders()
        {
            foreach (var item in MenuItems.OfType<NavigationViewItemHeader>())
            {
                switch (ItemHeaderBehavior)
                {
                    case ItemHeaderBehaviors.Hide:
                        item.Opacity = IsPaneOpen ? 1 : 0;
                        break;
                    case ItemHeaderBehaviors.Remove:
                        item.Visibility = IsPaneOpen ? Visibility.Visible : Visibility.Collapsed;
                        break;
                    case ItemHeaderBehaviors.None:
                        // empty
                        break;
                }
            }
        }

        private void UpdatePageHeader()
        {
            if (_frame.Content is Page p && p.GetValue(NavViewProps.HeaderTextProperty) is string s && !string.IsNullOrEmpty(s))
            {
                Header = s;
            }

            var children = XamlUtilities.RecurseChildren(this);
            if (!children.Any())
            {
                return;
            }

            var grids = children.OfType<Grid>();
            var grid = grids.Single(x => x.Name == "ContentGrid");
            var content = grid.Children.OfType<ContentPresenter>().Single();
            var header = grid.Children.OfType<ContentControl>().Single();

            switch (PageHeaderBehavior)
            {
                case PageHeaderBehaviors.Adjacent:
                    header.Visibility = Visibility.Visible;
                    content.SetValue(Grid.RowProperty, 1);
                    content.SetValue(Grid.RowSpanProperty, 1);
                    content.SetValue(Canvas.ZIndexProperty, 0);
                    break;
                case PageHeaderBehaviors.Overlay:
                    header.Visibility = Visibility.Visible;
                    content.SetValue(Grid.RowProperty, 0);
                    content.SetValue(Grid.RowSpanProperty, 3);
                    content.SetValue(Canvas.ZIndexProperty, -1);
                    break;
                case PageHeaderBehaviors.Collapsed:
                    header.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private bool TryFindItem(Type type, out object item)
        {
            // registered?

            if (!_registry.TryGetInfo(type, out var info))
            {
                item = null;
                return false;
            }

            // search settings

            if (NavigationQueue.TryParse(SettingsNavigationUri, null, out var settings))
            {
                if (type == settings.Last().PageType)
                {
                    item = SettingsItem;
                    return false;
                }
                else
                {
                    // not settings
                }
            }

            // filter menu items

            var menuItems = MenuItems
                .OfType<NavigationViewItem>()
                .Select(x => new
                {
                    Item = x,
                    Path = x.GetValue(NavViewProps.NavigationUriProperty) as string
                })
                .Where(x => !string.IsNullOrEmpty(x.Path));

            // search filtered items

            foreach (var menuItem in menuItems)
            {
                if (NavigationQueue.TryParse(menuItem.Path, null, out var menuQueue)
                    && Equals(menuQueue.Last().PageType, type))
                {
                    item = menuItem;
                    return true;
                }
            }

            // not found

            item = null;
            return false;
        }

        private NavigationViewItem Find(string content)
        {
            return MenuItems.OfType<NavigationViewItem>().SingleOrDefault(x => x.Content.Equals(content));
        }
    }
}
