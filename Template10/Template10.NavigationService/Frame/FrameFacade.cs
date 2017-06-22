using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Common.PersistedDictionary;
using Template10.Portable;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Services.NavigationService
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Navigation-Service
    public partial class FrameFacade : IFrameFacadeInternal, IFrameFacade
    {
        #region Debug

        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            LoggingService.LoggingService.WriteLine(text, severity, caller: $"{nameof(FrameFacade)}.{caller}");

        #endregion

        // [Obsolete("Internal use only")]
        internal FrameFacade(Frame frame, INavigationService navigationService)
        {
            (this as IFrameFacadeInternal).Frame = frame;
            frame.Navigating += (s, e) =>
            {
                if (!BackStack.Any() && string.IsNullOrEmpty(FrameId))
                {
                    // auto-assign the FrameId if it isn't assigned
                    FrameId = e.SourcePageType.ToString();
                }
            };

            (this as IFrameFacadeInternal).NavigationService = navigationService;

            // setup animations
            var t = new NavigationThemeTransition
            {
                DefaultNavigationTransitionInfo = new EntranceNavigationTransitionInfo()
            };
            (this as IFrameFacadeInternal).Frame.ContentTransitions = new TransitionCollection { };
            (this as IFrameFacadeInternal).Frame.ContentTransitions.Add(t);
        }

        public string FrameId { get; set; } = string.Empty;

        public object Content => (this as IFrameFacadeInternal).Frame.Content;

        public object GetValue(DependencyProperty dp) => (this as IFrameFacadeInternal).Frame.GetValue(dp);

        public void SetValue(DependencyProperty dp, object value) { (this as IFrameFacadeInternal).Frame.SetValue(dp, value); }

        public void ClearValue(DependencyProperty dp) { (this as IFrameFacadeInternal).Frame.ClearValue(dp); }

        public void ClearCache(bool removeCachedPagesInBackStack = false)
        {
            DebugWrite($"Frame: {FrameId}");

            var frame = (this as IFrameFacadeInternal).Frame;

            int currentSize = frame.CacheSize;
            try
            {
                if (removeCachedPagesInBackStack)
                {
                    frame.CacheSize = 0;
                }
                else
                {
                    if (frame.BackStackDepth == 0)
                    {
                        frame.CacheSize = 1;
                    }
                    else
                    {
                        frame.CacheSize = frame.BackStackDepth;
                    }
                }
            }
            catch
            {
                // Ignore exceptions here
            }
            finally
            {
                frame.CacheSize = currentSize;
            }
        }

        internal bool Navigate(Type page, object parameter) => (this as IFrameFacadeInternal).Frame.Navigate(page, parameter);

        public IList<PageStackEntry> BackStack => (this as IFrameFacadeInternal).Frame.BackStack;

        public IList<PageStackEntry> ForwardStack => (this as IFrameFacadeInternal).Frame.ForwardStack;

    }

    //public partial class FrameFacade : IFrameFacadeInternal
    //{
    //    // Obsolete properties/methods

    //    [Obsolete("This may be made private in a future version.", false)]
    //    public void GoBack() => (this as IFrameFacadeInternal).GoBack();

    //    [Obsolete("This may be made private in a future version.", false)]
    //    public void GoForward() => (this as IFrameFacadeInternal).GoForward();

    //    [Obsolete("Use FrameFacade.BackStack.Count. THis will be deleted in future versions.", false)]
    //    public int BackStackDepth => (this as IFrameFacadeInternal).Frame.BackStack.Count;

    //    [Obsolete("Use NavigationService.BackButtonHandling This may be made private in a future version.", false)]
    //    public BackButton BackButtonHandling
    //    {
    //        get { return NavigationService.BackButtonHandling; }
    //        internal set { NavigationService.BackButtonHandling = value; }
    //    }

    //    [Obsolete("Use NavigationService.Suspension.GetFrameState(). This may be made private in a future version.", false)]
    //    private SettingsService.ISettingsService FrameStateSettingsService() { throw new NotImplementedException(); }

    //    [Obsolete("Use NavigationService.Suspension.GetFrameState().Write(). This may be made private in a future version.", false)]
    //    public void SetFrameState(string key, string value) { throw new NotImplementedException(); }

    //    [Obsolete("Use NavigationService.Suspension.GetFrameState().Read(). This may be made private in a future version.", false)]
    //    public string GetFrameState(string key, string otherwise) { throw new NotImplementedException(); }

    //    [Obsolete("Use NavigationService.Suspension.ClearFrameState(). This may be made private in a future version.", false)]
    //    public void ClearFrameState() { throw new NotImplementedException(); }

    //    [Obsolete("Use NavigationService.Suspension.GetPageState(). This may be made private in a future version.", false)]
    //    public SettingsService.ISettingsService PageStateSettingsService(Type type) { throw new NotImplementedException(); }

    //    [Obsolete("Use NavigationService.Suspension.GetPageState(). This may be made private in a future version.", false)]
    //    public SettingsService.ISettingsService PageStateSettingsService(string key) { throw new NotImplementedException(); }

    //    [Obsolete("Use NavigationService.Suspension.ClearPageState(). This may be made private in a future version.", false)]
    //    public void ClearPageState(Type type) { throw new NotImplementedException(); }

    //    [Obsolete("This may be made private in a future version.", false)]
    //    public INavigationService NavigationService { get { throw new NotImplementedException(); } }

    //    [Obsolete("This will be made private in a future version.", true)]
    //    public Frame Frame { get { throw new NotImplementedException(); } }

    //    [Obsolete("This may be made private in a future version.", true)]
    //    public NavigationMode NavigationModeHint { get { throw new NotImplementedException(); } }

    //    [Obsolete("Use NavigationService.Refresh(). This may be made private in a future version.", false)]
    //    public void Refresh() { throw new NotImplementedException(); }

    //    [Obsolete("Use NavigationService.Refresh(). This may be made private in a future version.", false)]
    //    public void Refresh(object param) { throw new NotImplementedException(); }

    //    [Obsolete("Use NavigationService.LastNavigationType. This may be made private in a future version.", false)]
    //    public Type CurrentPageType { get { throw new NotImplementedException(); } }

    //    [Obsolete("Use NavigationService.LastNavigationParameter. This may be made private in a future version.", false)]
    //    public object CurrentPageParam { get { throw new NotImplementedException(); } }

    //    [Obsolete("Use NavigationService.SerializationService. This may be made private in a future version.", false)]
    //    public SerializationService.ISerializationService SerializationService { get { throw new NotImplementedException(); } }
    //}

    public partial class FrameFacade : IFrameFacadeInternal
    {
        // Internal properties/methods

        string FrameStateKey => $"Frame-{FrameId ?? "DefaultFrame"}";

        async Task<FrameState> IFrameFacadeInternal.GetFrameStateAsync()
        {
            var store = await PersistedDictionaryHelper.GetAsync(FrameStateKey, string.Empty, Settings.PersistenceDefault);
            return new FrameState(store);
        }

        async Task<IPersistedDictionary> IFrameFacadeInternal.GetPageStateAsync(Type page)
        {
            if (page == null) return null;
            return await PersistedDictionaryHelper.GetAsync($"Page-{page.ToString()}", FrameStateKey, Settings.PersistenceDefault);
        }

        bool IFrameFacadeInternal.CanGoBack => (this as IFrameFacadeInternal).Frame.CanGoBack;

        bool IFrameFacadeInternal.CanGoForward => (this as IFrameFacadeInternal).Frame.CanGoForward;

        void IFrameFacadeInternal.SetNavigationState(string state) => (this as IFrameFacadeInternal).Frame.SetNavigationState(state);

        string IFrameFacadeInternal.GetNavigationState() => (this as IFrameFacadeInternal).Frame.GetNavigationState();

        void IFrameFacadeInternal.GoForward()
        {
            try
            {
                if ((this as IFrameFacadeInternal).CanGoForward)
                {
                    (this as IFrameFacadeInternal).Frame.GoForward();
                }
            }
            catch (Exception)
            {
                Debugger.Break();
                throw;
            }
        }

        bool IFrameFacadeInternal.Navigate(Type page, object parameter, NavigationTransitionInfo info) => (this as IFrameFacadeInternal).Frame.Navigate(page, parameter, info);

        void IFrameFacadeInternal.GoBack(NavigationTransitionInfo infoOverride)
        {
            try
            {
                if ((this as IFrameFacadeInternal).CanGoBack)
                {
                    (this as IFrameFacadeInternal).Frame.GoBack(infoOverride);
                }
            }
            catch (Exception)
            {
                Debugger.Break();
                throw;
            }
        }

        void IFrameFacadeInternal.GoBack()
        {
            try
            {
                if ((this as IFrameFacadeInternal).CanGoBack)
                {
                    (this as IFrameFacadeInternal).Frame.GoBack();
                }
            }
            catch (Exception)
            {
                Debugger.Break();
                throw;
            }
        }

        Frame IFrameFacadeInternal.Frame { get; set; }

        INavigationService IFrameFacadeInternal.NavigationService { get; set; }
    }
}
