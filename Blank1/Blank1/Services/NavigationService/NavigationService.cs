using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Blank1.Services.NavigationService
{
    public class NavigationService
    {
        private readonly FrameFacade _frame;
        private const string EmptyNavigation = "1,0";

        string LastNavigationParameter { get; set; }
        string LastNavigationType { get; set; }

        public NavigationService(Frame frame)
        {
            _frame = new FrameFacade(frame);
            _frame.Navigating += async (s, e) =>
            {
                if (e.Suspending)
                    return;

                // allow the viewmodel to cancel navigation
                e.Cancel = !NavigatingFrom(false);
                if (!e.Cancel)
                {
                    await NavigateFromAsync(false);
                }
            };
            _frame.Navigated += (s, e) =>
            {
                NavigateTo(e.NavigationMode, e.Parameter);
            };
        }

        public Windows.Storage.ApplicationDataContainer State()
        {
            var data = Windows.Storage.ApplicationData.Current;
            var app = data.LocalSettings.CreateContainer("PageState", Windows.Storage.ApplicationDataCreateDisposition.Always);
            return app;
        }

        public IPropertySet State(Type type)
        {
            var key = string.Format("{0}", type);
            var container = State().CreateContainer(key, Windows.Storage.ApplicationDataCreateDisposition.Always);
            return container.Values;
        }

        bool NavigatingFrom(bool suspending)
        {
            var page = _frame.Content as FrameworkElement;
            if (page != null)
            {
                var dataContext = page.DataContext as INavigatable;
                if (dataContext != null)
                {
                    var args = new NavigatingEventArgs
                    {
                        PageType = _frame.CurrentPageType,
                        Parameter = _frame.CurrentPageParam,
                        Suspending = suspending,
                    };
                    dataContext.OnNavigatingFrom(args);
                    return !args.Cancel;
                }
            }
            return true;
        }

        async Task NavigateFromAsync(bool suspending)
        {
            var page = _frame.Content as FrameworkElement;
            if (page != null)
            {
                var dataContext = page.DataContext as INavigatable;
                if (dataContext != null)
                {
                    await dataContext.OnNavigatedFromAsync(State(CurrentPageType), suspending);
                }
            }
        }

        void NavigateTo(NavigationMode mode, string parameter)
        {
            LastNavigationParameter = parameter;
            LastNavigationType = _frame.Content.GetType().FullName;

            if (mode == NavigationMode.New)
            {
                // clear state
                var state = State();
                foreach (var container in state.Containers)
                {
                    state.DeleteContainer(container.Key);
                }
            }

            var page = _frame.Content as FrameworkElement;
            if (page != null)
            {
                var dataContext = page.DataContext as INavigatable;
                if (dataContext != null)
                {
                    var state = State(page.GetType());
                    dataContext.OnNavigatedTo(parameter, mode, state);
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

        public void SaveNavigation()
        {
            var state = State(GetType());
            state["CurrentPageType"] = CurrentPageType.ToString();
            state["CurrentPageParam"] = CurrentPageParam;
            state["NavigateState"] = _frame.GetNavigationState();
        }

        public bool RestoreSavedNavigation()
        {
            try
            {
                var state = State(GetType());
                _frame.CurrentPageType = Type.GetType(state["CurrentPageType"].ToString());
                _frame.CurrentPageParam = state["CurrentPageParam"]?.ToString();
                _frame.SetNavigationState(state["NavigateState"].ToString());
                NavigateTo(NavigationMode.Refresh, _frame.CurrentPageParam);
                return true;
            }
            catch { return false; }
        }

        public void GoBack() { if (_frame.CanGoBack) _frame.GoBack(); }

        public bool CanGoBack { get { return _frame.CanGoBack; } }

        public void GoForward() { _frame.GoForward(); }

        public bool CanGoForward { get { return _frame.CanGoForward; } }

        public void ClearHistory() { _frame.SetNavigationState(EmptyNavigation); }

        public void Resuming() { /* nothing */ }

        public async Task SuspendingAsync()
        {
            SaveNavigation();
            await NavigateFromAsync(true);
        }

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

