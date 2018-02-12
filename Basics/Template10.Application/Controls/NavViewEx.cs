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
using win = Windows;
using System.Threading;

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

            if (win.ApplicationModel.DesignMode.DesignModeEnabled
                || win.ApplicationModel.DesignMode.DesignMode2Enabled)
            {
                return;
            }

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
            };
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

        private static SemaphoreSlim _updatePageHeaderSemaphore = new SemaphoreSlim(1, 1);

        private void UpdatePageHeader()
        {
            _updatePageHeaderSemaphore.Wait();

            bool localTryGetHeaderAndContent(out ContentPresenter content, out ContentControl header)
            {
                var children = XamlUtilities.RecurseChildren(this);
                var grids = children
                    .OfType<Grid>()
                    .Where(x => x.Name == "ContentGrid");
                if (!grids.Any())
                {
                    content = default(ContentPresenter);
                    header = default(ContentControl);
                    return false;
                }
                var grid = grids.Single();
                content = grid.Children.OfType<ContentPresenter>().Single();
                header = grid.Children.OfType<ContentControl>().Single();
                return true;
            }

            void localUpdatePageHeaderBehavior()
            {
                if (!localTryGetHeaderAndContent(out var content, out var header))
                {
                    return;
                }

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

            bool localTryGetCommandBar(out CommandBar bar)
            {
                var children = XamlUtilities.RecurseChildren(this);
                var bars = children
                    .Where(x => x.GetValue(NavViewProps.PageHeaderCommandsMergeTargetProperty) is bool value && value)
                    .OfType<CommandBar>();
                if (!bars.Any())
                {
                    bar = default(CommandBar);
                    return false;
                }
                bar = bars.Single();
                return true;
            }

            void localUpdatePageHeaderCommands(ObservableCollection<object> headerCommands)
            {
                if (!localTryGetCommandBar(out var bar))
                {
                    return;
                }

                var previous = bar.PrimaryCommands
                    .OfType<DependencyObject>()
                    .Where(x => x.GetValue(NavViewProps.PageHeaderCommandDynamicItemProperty) is bool value && value);

                foreach (var command in previous.OfType<ICommandBarElement>().ToArray())
                {
                    bar.PrimaryCommands.Remove(command);
                }

                foreach (var command in headerCommands.Reverse().OfType<DependencyObject>().ToArray())
                {
                    command.SetValue(NavViewProps.PageHeaderCommandDynamicItemProperty, true);
                    bar.PrimaryCommands.Insert(0, command as ICommandBarElement);
                }
            }

            try
            {
                if (_frame.Content is Page page)
                {
                    if (page.GetValue(NavViewProps.HeaderTextProperty) is string headerText && !Equals(Header, headerText))
                    {
                        Header = headerText;
                        localUpdatePageHeaderBehavior();
                    }

                    if (page.GetValue(NavViewProps.HeaderCommandsProperty) is ObservableCollection<object> headerCommands && headerCommands.Any())
                    {
                        localUpdatePageHeaderCommands(headerCommands);
                    }
                }
            }
            finally
            {
                _updatePageHeaderSemaphore.Release();
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
