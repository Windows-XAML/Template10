using System;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public class NavigationService
    {
        private readonly NavigationFacade _frame;
        private const string EmptyNavigation = "1,0";

        string LastNavigationParameter { get; set; /* TODO: persist */ }
        string LastNavigationType { get; set; /* TODO: persist */ }

        public NavigationService(Frame frame)
        {
            _frame = new NavigationFacade(frame);
            _frame.Navigating += (s, e) => NavigateFrom(false);
            _frame.Navigated += (s, e) => NavigateTo(e.NavigationMode, e.Parameter);
        }

        public IPropertySet State(Type type)
        {
            var key = string.Format("{0}", type);
            var data = Windows.Storage.ApplicationData.Current;
            var page = data.LocalSettings.CreateContainer("PageState", Windows.Storage.ApplicationDataCreateDisposition.Always);
            var container = page.CreateContainer(key, Windows.Storage.ApplicationDataCreateDisposition.Always);
            return container.Values;
        }

        void NavigateFrom(bool suspending)
        {
            var page = _frame.Content as FrameworkElement;
            if (page != null)
            {
                var dataContext = page.DataContext as INavigatable;
                if (dataContext != null)
                {
                    dataContext.OnNavigatedFrom(State(this.CurrentPageType), suspending);
                }
            }
        }

        void NavigateTo(NavigationMode mode, string parameter)
        {
            LastNavigationParameter = parameter;
            LastNavigationType = _frame.Content.GetType().FullName;

            if (mode == NavigationMode.New)
            {
                // TODO: clear existing state
            }

            var page = _frame.Content as FrameworkElement;
            if (page != null)
            {
                var dataContext = page.DataContext as INavigatable;
                if (dataContext != null)
                {
                    dataContext.OnNavigatedTo(parameter, mode, State(page.GetType()));
                }
            }
        }

        public bool Navigate(Type page, string parameter = null)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));
            if (page.FullName.Equals(LastNavigationType)
                && parameter == LastNavigationParameter)
                return false;
            return _frame.Navigate(page, parameter);
        }

        public void RestoreSavedNavigation() { /* TODO */ }

        public void GoBack() { if (_frame.CanGoBack) _frame.GoBack(); }

        public bool CanGoBack { get { return _frame.CanGoBack; } }

        public void GoForward() { _frame.GoForward(); }

        public bool CanGoForward { get { return _frame.CanGoForward; } }

        public void ClearHistory() { _frame.SetNavigationState(EmptyNavigation); }

        public void Suspending() { NavigateFrom(true); }

        public void Show(SettingsFlyout flyout, string parameter = null)
        {
            if (flyout == null)
                throw new ArgumentNullException(nameof(flyout));
            var dataContext = flyout.DataContext as INavigatable;
            if (dataContext != null)
            {
                dataContext.OnNavigatedTo(parameter, NavigationMode.New, null);
            }
            flyout.Show();
        }

        public Type CurrentPageType { get { return _frame.CurrentPageType; } }

        public string CurrentPageParam { get { return _frame.CurrentPageParam; } }
    }
}

