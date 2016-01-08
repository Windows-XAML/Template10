﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    using System.Text;
    using Windows.Foundation.Collections;
    using Windows.Storage;
    using Windows.UI.Xaml.Data;

    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-NavigationService
    public partial class NavigationService : INavigationService
    {
        private const string EmptyNavigation = "1,0";

        public FrameFacade FrameFacade { get; }
        public Frame Frame => FrameFacade.Frame;
        object LastNavigationParameter { get; set; }
        string LastNavigationType { get; set; }

        public DispatcherWrapper Dispatcher => WindowWrapper.Current(this).Dispatcher;

        protected internal NavigationService(Frame frame)
        {
            FrameFacade = new FrameFacade(frame);
            FrameFacade.Navigating += async (s, e) =>
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
            FrameFacade.Navigated += (s, e) =>
            {
                NavigateTo(e.NavigationMode, ParameterSerializationService.Instance.DeserializeParameter(e.Parameter));
            };
        }

        // before navigate (cancellable) 
        bool NavigatingFrom(bool suspending)
        {
            var page = FrameFacade.Content as Page;
            if (page != null)
            {
                // force (x:bind) page bindings to update
                var fields = page.GetType().GetRuntimeFields();
                var bindings = fields.FirstOrDefault(x => x.Name.Equals("Bindings"));
                if (bindings != null)
                {
                    var update = bindings.GetType().GetTypeInfo().GetDeclaredMethod("Update");
                    update?.Invoke(bindings, null);
                }

                // call navagable override (navigating)
                var dataContext = page.DataContext as INavigable;
                if (dataContext != null)
                {
                    var args = new NavigatingEventArgs
                    {
                        NavigationMode = FrameFacade.NavigationModeHint,
                        PageType = FrameFacade.CurrentPageType,
                        Parameter = FrameFacade.CurrentPageParam,
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
            var page = FrameFacade.Content as Page;
            if (page != null)
            {
                // call viewmodel
                var dataContext = page.DataContext as INavigable;
                if (dataContext != null)
                {
                    var pageState = FrameFacade.PageStateContainer(page.GetType());
                    await dataContext.OnNavigatedFromAsync(pageState, suspending);
                }
            }
        }

        void NavigateTo(NavigationMode mode, object parameter)
        {
            LastNavigationParameter = parameter;
            LastNavigationType = FrameFacade.Content.GetType().FullName;

            if (mode == NavigationMode.New)
            {
                FrameFacade.ClearFrameState();
            }

            var page = FrameFacade.Content as Page;
            if (page != null)
            {
                if (page.DataContext == null)
                {
                    // to support dependency injection, but keeping it optional.
                    var viewmodel = BootStrapper.Current.ResolveForPage(page.GetType(), this);
                    if (viewmodel != null)
                        page.DataContext = viewmodel;
                }

                // call viewmodel
                var dataContext = page.DataContext as INavigable;
                if (dataContext != null)
                {
                    // prepare for state load
                    dataContext.NavigationService = this;
                    dataContext.Dispatcher = Common.WindowWrapper.Current(this)?.Dispatcher;
                    dataContext.SessionState = BootStrapper.Current.SessionState;
                    var pageState = FrameFacade.PageStateContainer(page.GetType());
                    dataContext.OnNavigatedTo(parameter, mode, pageState);
                }
            }
        }

        public async Task OpenAsync(Type page, object parameter = null, string title = null, ViewSizePreference size = ViewSizePreference.UseHalf)
        {
            var currentView = ApplicationView.GetForCurrentView();
            title = title ?? currentView.Title;

            var newView = CoreApplication.CreateNewView();
            var dispatcher = new DispatcherWrapper(newView.Dispatcher);
            await dispatcher.DispatchAsync(async () =>
            {
                var newWindow = Window.Current;
                var newAppView = ApplicationView.GetForCurrentView();
                newAppView.Title = title;

                var frame = BootStrapper.Current.NavigationServiceFactory(BootStrapper.BackButton.Ignore, BootStrapper.ExistingContent.Exclude);
                frame.Navigate(page, parameter);
                newWindow.Content = frame.Frame;
                newWindow.Activate();

                await ApplicationViewSwitcher
                    .TryShowAsStandaloneAsync(newAppView.Id, ViewSizePreference.Default, currentView.Id, size);
            });
        }

        public bool Navigate(Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));
            if (page.FullName.Equals(LastNavigationType))
            {
                if (parameter == LastNavigationParameter)
                    return false;

                if (parameter != null && parameter.Equals(LastNavigationParameter))
                    return false;
            }

            parameter = ParameterSerializationService.Instance.SerializeParameter(parameter);
            return FrameFacade.Navigate(page, parameter, infoOverride);
        }

        /*
            Navigate<T> allows developers to navigate using a
            page key instead of the view type. This is accomplished by
            creating a custom Enum and setting up the PageKeys dict
            with the Key/Type pairs for your views. The dict is
            shared by all NavigationServices and is stored in
            the BootStrapper (or Application) of the app.

            Implementation example:

            // define your Enum
            public Enum Pages { MainPage, DetailPage }

            // setup the keys dict
            var keys = BootStrapper.PageKeys<Views>();
            keys.Add(Pages.MainPage, typeof(Views.MainPage));
            keys.Add(Pages.DetailPage, typeof(Views.DetailPage));

            // use Navigate<T>()
            NavigationService.Navigate(Pages.MainPage);
        */

        // T must be the same custom Enum used with BootStrapper.PageKeys()
        public bool Navigate<T>(T key, object parameter = null, NavigationTransitionInfo infoOverride = null)
            where T : struct, IConvertible
        {
            var keys = Common.BootStrapper.Current.PageKeys<T>();
            if (!keys.ContainsKey(key))
                throw new KeyNotFoundException(key.ToString());
            var page = keys[key];
            if (page.FullName.Equals(LastNavigationType)
                && parameter == LastNavigationParameter)
                return false;
            return FrameFacade.Navigate(page, parameter, infoOverride);
        }

        public void SaveNavigation()
        {
            if (CurrentPageType == null)
                return;

            var state = FrameFacade.PageStateContainer(GetType());
            if (state == null)
            {
                throw new InvalidOperationException("State container is unexpectedly null");
            }

            string pageTypeValue = this.CurrentPageType.AssemblyQualifiedName;
            object pageParamValue = ParameterSerializationService.Instance.SerializeParameter(this.CurrentPageParam);
            string navigationStateValue = this.FrameFacade?.GetNavigationState();

            state["CurrentPageType"] = pageTypeValue;
            SaveLongStateValue(state, "CurrentPageParam", pageParamValue);
            SaveLongStateValue(state, "NavigateState", navigationStateValue);
        }

        public event TypedEventHandler<Type> AfterRestoreSavedNavigation;
        public bool RestoreSavedNavigation()
        {
            try
            {
                var state = FrameFacade.PageStateContainer(GetType());
                if (state == null || !state.Any() || !state.ContainsKey("CurrentPageType"))
                {
                    return false;
                }

                string pageTypeValue = state["CurrentPageType"].ToString();
                object pageParamValue = RestoreLongStateValue(state, "CurrentPageParam");
                string navigationStateValue = RestoreLongStateValue(state, "NavigateState")?.ToString();

                FrameFacade.CurrentPageType = Type.GetType(pageTypeValue);
                FrameFacade.CurrentPageParam = ParameterSerializationService.Instance.DeserializeParameter(pageParamValue);
                FrameFacade.SetNavigationState(navigationStateValue);
                NavigateTo(NavigationMode.Refresh, FrameFacade.CurrentPageParam);
                while (Frame.Content == null)
                {
                    Task.Yield().GetAwaiter().GetResult();
                }
                AfterRestoreSavedNavigation?.Invoke(this, FrameFacade.CurrentPageType);
                return true;
            }
            catch { return false; }
        }

        private const int MaxValueSize = 8000;
        private const int MaxCompositeSize = 64000;

        private static void SaveLongStateValue(IPropertySet state, string key, object value)
        {
            var stringValue = value?.ToString();
            if (string.IsNullOrEmpty(stringValue) || (stringValue.Length <= MaxValueSize))
            {
                state[key] = value;
            }
            else
            {
                int remainingLength = stringValue.Length;
                if (remainingLength > MaxCompositeSize)
                {
                    throw new Exception("Value too large.");
                }
                int statePartCount = (remainingLength - 1) / MaxValueSize + 1;

                // Create composite value
                var compositeValue = new ApplicationDataCompositeValue();
                compositeValue["Count"] = statePartCount.ToString();
                state[key] = compositeValue;

                // Split string into parts
                for (int statePart = 0; statePart < statePartCount; statePart++)
                {
                    string statePartValue = stringValue.Substring(statePart * MaxValueSize, Math.Min(MaxValueSize, remainingLength));
                    compositeValue["Part" + statePart] = statePartValue;
                    remainingLength = remainingLength - MaxValueSize;
                }
            }
        }

        private static object RestoreLongStateValue(IPropertySet state, string key)
        {
            object value = state[key];
            var compositeValue = value as ApplicationDataCompositeValue;
            if (compositeValue != null)
            {
                string statePartCountValue = compositeValue["Count"]?.ToString();
                int statePartCount;
                if (!string.IsNullOrEmpty(statePartCountValue) && int.TryParse(statePartCountValue, out statePartCount))
                {
                    var sb = new StringBuilder(statePartCount * MaxValueSize);
                    for (int statePart = 0; statePart < statePartCount; statePart++)
                    {
                        sb.Append(compositeValue["Part" + statePart]);
                    }
                    value = sb.ToString();
                }
            }
            return value;
        }

        public void Refresh() { FrameFacade.Refresh(); }

        public void GoBack() { if (FrameFacade.CanGoBack) FrameFacade.GoBack(); }

        public bool CanGoBack => FrameFacade.CanGoBack;

        public void GoForward() { FrameFacade.GoForward(); }

        public bool CanGoForward => FrameFacade.CanGoForward;

        public void ClearCache(bool removeCachedPagesInBackStack = false)
        {
            int currentSize = FrameFacade.Frame.CacheSize;

            if (removeCachedPagesInBackStack)
            {
                FrameFacade.Frame.CacheSize = 0;
            }
            else
            {
                if (Frame.BackStackDepth == 0)
                    Frame.CacheSize = 1;
                else
                    Frame.CacheSize = Frame.BackStackDepth;
            }

            FrameFacade.Frame.CacheSize = currentSize;
        }

        public void ClearHistory() { FrameFacade.Frame.BackStack.Clear(); }

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

        public Type CurrentPageType => FrameFacade.CurrentPageType;
        public object CurrentPageParam => FrameFacade.CurrentPageParam;
    }
}

