using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    public class NavigationService
    {
        private const string EmptyNavigation = "1,0";

        public FrameFacade Frame { get; private set; }
        string LastNavigationParameter { get; set; }
        string LastNavigationType { get; set; }

        public NavigationService(Frame frame)
        {
            Frame = new FrameFacade(frame);
            Frame.Navigating += async (s, e) =>
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
            Frame.Navigated += (s, e) =>
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

        // before navigate (cancellable)
        bool NavigatingFrom(bool suspending)
        {
            var page = Frame.Content as Page;
            if (page != null)
            {
                var dataContext = page.DataContext as INavigable;
                if (dataContext != null)
                {
                    var args = new NavigatingEventArgs
                    {
                        PageType = Frame.CurrentPageType,
                        Parameter = Frame.CurrentPageParam,
                        Suspending = suspending,
                    };
                    dataContext.OnNavigatingFrom(args);
                    return !args.Cancel;
                }
            }
            return true;
        }

        // after navigate
        async Task NavigateFromAsync(bool suspending)
        {
            var page = Frame.Content as Page;
            if (page != null)
            {
                // call viewmodel
                var dataContext = page.DataContext as INavigable;
                if (dataContext != null)
                {
                    dataContext.ViewModelIdentifier = string.Format("Page- {0}", Frame.BackStackDepth);
                    await dataContext.OnNavigatedFromAsync(State(CurrentPageType), suspending);
                }
            }
        }

        void NavigateTo(NavigationMode mode, string parameter)
        {
            LastNavigationParameter = parameter;
            LastNavigationType = Frame.Content.GetType().FullName;

            if (mode == NavigationMode.New)
            {
                // clear state
                var state = State();
                foreach (var container in state.Containers)
                {
                    state.DeleteContainer(container.Key);
                }
            }

            var page = Frame.Content as Page;
            if (page != null)
            {
                // call viewmodel
                var dataContext = page.DataContext as INavigable;
                if (dataContext != null)
                {
                    if (dataContext.ViewModelIdentifier != null && (mode == NavigationMode.Forward || mode == NavigationMode.Back))
                    {
                        // don't call load if cached && navigating back/forward
                        return;
                    }
                    else
                    {
                        // setup dispatcher to correct thread
                        var dispatch = new Action<Action>(async action =>
                        {
                            if (page.Dispatcher.HasThreadAccess) { action(); }
                            else { await page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => action()); }
                        });
                        dataContext.Dispatch = dispatch;

                        // prepare for state load
                        var state = State(page.GetType());
                        dataContext.OnNavigatedTo(parameter, mode, state);
                    }
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
            return Frame.Navigate(page, parameter);
        }

        public void SaveNavigation()
        {
            var state = State(GetType());
            state["CurrentPageType"] = CurrentPageType.ToString();
            state["CurrentPageParam"] = CurrentPageParam;
            state["NavigateState"] = Frame.GetNavigationState();
        }

        public bool RestoreSavedNavigation()
        {
            try
            {
                var state = State(GetType());
                Frame.CurrentPageType = Type.GetType(state["CurrentPageType"].ToString());
                Frame.CurrentPageParam = state["CurrentPageParam"]?.ToString();
                Frame.SetNavigationState(state["NavigateState"].ToString());
                NavigateTo(NavigationMode.Refresh, Frame.CurrentPageParam);
                return true;
            }
            catch { return false; }
        }

        public void GoBack() { if (Frame.CanGoBack) Frame.GoBack(); }

        public bool CanGoBack { get { return Frame.CanGoBack; } }

        public void GoForward() { Frame.GoForward(); }

        public bool CanGoForward { get { return Frame.CanGoForward; } }

        public void ClearHistory() { Frame.SetNavigationState(EmptyNavigation); }

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
            var dataContext = flyout.DataContext as INavigable;
            if (dataContext != null)
            {
                dataContext.OnNavigatedTo(parameter, NavigationMode.New, null);
            }
            flyout.Show();
        }

        public Type CurrentPageType { get { return Frame.CurrentPageType; } }
        public string CurrentPageParam { get { return Frame.CurrentPageParam; } }
    }
}

