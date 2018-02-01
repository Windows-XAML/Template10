using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Application.Services;
using Template10.Navigation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Template10.Controls
{
    public class NavViewEx : NavigationView
    {
        public INavigationServiceUwp NavigationService { get; }

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

            ItemInvoked += (s, e) => SelectedItem = (e.IsSettingsInvoked) ? SettingsItem : Find(e.InvokedItem.ToString());
            RegisterPropertyChangedCallback(IsPaneOpenProperty, (s, e) => UpdatePaneHeaders());

            Loaded += (s, e) =>
            {
                UpdatePaneHeaders();
                UpdateBackButton();
                UpdatePageHeader();
            };
        }

        public INavigationServiceUwp Start()
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

        public enum BackButtonBehaviors { Auto, Visible, Collapsed }

        public BackButtonBehaviors BackButtonBehavior { get; set; } = BackButtonBehaviors.Auto;

        public enum ItemHeaderBehaviors { Hide, Remove, None }

        public ItemHeaderBehaviors ItemHeaderBehavior { get; set; } = ItemHeaderBehaviors.Remove;

        public enum PageHeaderBehaviors { Below, Behind, Remove }

        public PageHeaderBehaviors PageHeaderBehavior { get; set; } = PageHeaderBehaviors.Below;

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
        }

        private void UpdateBackButton()
        {
            switch (BackButtonBehavior)
            {
                case BackButtonBehaviors.Auto:
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                            (_frame.CanGoBack)
                            ? AppViewBackButtonVisibility.Visible
                            : AppViewBackButtonVisibility.Collapsed;
                    break;
                case BackButtonBehaviors.Visible:
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                    break;
                case BackButtonBehaviors.Collapsed:
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

            var children = Utilities.XamlUtilities.RecurseChildren(this);
            if (children.Any())
            {
                var child = children.Single(x => x.Name == "ContentGrid");
                if (child is Grid grid && grid != null)
                {
                    var presenter = grid.Children.OfType<ContentPresenter>().Single();
                    var header = grid.Children.OfType<ContentControl>().Single();
                    switch (PageHeaderBehavior)
                    {
                        case PageHeaderBehaviors.Below:
                            header.Visibility = Visibility.Visible;
                            presenter.SetValue(Grid.RowProperty, 1);
                            presenter.SetValue(Grid.RowSpanProperty, 1);
                            presenter.SetValue(Canvas.ZIndexProperty, 0);
                            break;
                        case PageHeaderBehaviors.Behind:
                            header.Visibility = Visibility.Visible;
                            presenter.SetValue(Grid.RowProperty, 0);
                            presenter.SetValue(Grid.RowSpanProperty, 2);
                            presenter.SetValue(Canvas.ZIndexProperty, -1);
                            break;
                        case PageHeaderBehaviors.Remove:
                            header.Visibility = Visibility.Collapsed;
                            break;
                    }
                }
            }
        }

        private bool TryFindItem(Type type, out object item)
        {
            // registered?

            if (!NavigationRegistry.TryGetInfo(type, out var info))
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
