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
using Windows.UI.Xaml.Navigation;

namespace Template10.Controls
{
    public class NavViewEx : NavigationView
    {
        private INavigationServiceUwp _navigationService;

        public NavViewEx()
        {
            DefaultStyleKey = typeof(NavigationView);

            Content = Frame = new Frame();
            Frame.Navigated += (s, e) =>
            {
                if (TryFindItem(e.SourcePageType, out var item))
                {
                    SetSelectedItem(item);
                }
            };

            _navigationService = new NavigationService(Frame);
            _navigationService.CanGoBackChanged += (s, e) =>
            {
                UpdateBackButton();
            };

            ItemInvoked += (s, e) => SelectedItem = (e.IsSettingsInvoked) ? SettingsItem : Find(e.InvokedItem.ToString());
            RegisterPropertyChangedCallback(IsPaneOpenProperty, (s, e) => UpdatePaneHeaders());

            GestureService.BackRequested += (s, e) =>
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
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
            UpdatePaneHeaders();
            UpdateBackButton();
            UpdatePageHeader();
            return _navigationService;
        }

        public enum BackButtonBehaviors { Auto, Visible, Collapsed }

        public BackButtonBehaviors BackButtonBehavior { get; set; } = BackButtonBehaviors.Auto;

        public enum PaneHeadersBehaviors { Hide, Remove, None }

        public PaneHeadersBehaviors PaneHeadersBehavior { get; set; } = PaneHeadersBehaviors.Remove;

        public Frame Frame { get; }

        public Uri SettingsNavigationUri { get; set; }

        public event EventHandler SettingsInvoked;

        public new object SelectedItem
        {
            set
            {
                SetSelectedItem(value);
                UpdatePaneHeaders();
                UpdateBackButton();
                UpdatePageHeader();
            }
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
                    _navigationService.NavigateAsync(SettingsNavigationUri).RunSynchronously();
                    base.SelectedItem = selectedItem;
                }
                SettingsInvoked?.Invoke(this, EventArgs.Empty);
            }
            else if (selectedItem is NavigationViewItem item)
            {
                if (item.GetValue(NavViewProps.NavigationUriProperty) is string path)
                {
                    if ((await _navigationService.NavigateAsync(path)).Success)
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
        }

        private void UpdateBackButton()
        {
            switch (BackButtonBehavior)
            {
                case BackButtonBehaviors.Auto:
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                            (Frame.CanGoBack)
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
                switch (PaneHeadersBehavior)
                {
                    case PaneHeadersBehaviors.Hide:
                        item.Opacity = IsPaneOpen ? 1 : 0;
                        break;
                    case PaneHeadersBehaviors.Remove:
                        item.Visibility = IsPaneOpen ? Visibility.Visible : Visibility.Collapsed;
                        break;
                    case PaneHeadersBehaviors.None:
                        // empty
                        break;
                }
            }
        }

        private bool TryFindItem(Type type, out object item)
        {
            // registered?

            if (!PageNavigationRegistry.TryGetInfo(type, out var info))
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

        private void UpdatePageHeader()
        {
            if (Frame.Content is Page p && p.GetValue(NavViewProps.HeaderTextProperty) is string s && !string.IsNullOrEmpty(s))
            {
                Header = s;
            }
        }

        private NavigationViewItem Find(string content)
        {
            return MenuItems.OfType<NavigationViewItem>().SingleOrDefault(x => x.Content.Equals(content));
        }
    }
}
