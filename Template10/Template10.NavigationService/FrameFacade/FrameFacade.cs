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
                //if (!BackStack.Any() && string.IsNullOrEmpty(FrameId))
                //{
                //    // auto-assign the FrameId if it isn't assigned
                //    FrameId = e.SourcePageType.ToString();
                //}
            };

            (this as IFrameFacadeInternal).NavigationService = navigationService;

            // setup animations
            SetupAnnimations();
        }

        private void SetupAnnimations()
        {
            var t = new NavigationThemeTransition
            {
                DefaultNavigationTransitionInfo = new EntranceNavigationTransitionInfo()
            };
            (this as IFrameFacadeInternal).Frame.ContentTransitions = new TransitionCollection { };
            (this as IFrameFacadeInternal).Frame.ContentTransitions.Add(t);
        }

        public string FrameId { get; set; } = "DefaultFrame";

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

        public void PurgeNavigationState() => (this as IFrameFacadeInternal).Frame.SetNavigationState("1,0");

        public IList<PageStackEntry> BackStack => (this as IFrameFacadeInternal).Frame.BackStack;

        public IList<PageStackEntry> ForwardStack => (this as IFrameFacadeInternal).Frame.ForwardStack;

    }

    public partial class FrameFacade : IFrameFacadeInternal
    {
        // Internal properties/methods

        string FrameStateKey => $"Frame-{FrameId}";

        async Task<FrameState> IFrameFacadeInternal.GetFrameStateAsync()
        {
            var store = await Settings.PersistedDictionaryFactory.CreateAsync(FrameStateKey);
            return new FrameState(store);
        }

        async Task<IPersistedDictionary> IFrameFacadeInternal.GetPageStateAsync(Type page)
        {
            if (page == null) return null;
            return await Settings.PersistedDictionaryFactory.CreateAsync($"Page-{page.ToString()}", FrameStateKey);
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
