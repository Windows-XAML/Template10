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
    public partial class FrameWrapper : IFrameWrapperInternal, IFrameWrapper
    {
        #region Debug

        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            LoggingService.LoggingService.WriteLine(text, severity, caller: $"{nameof(FrameWrapper)}.{caller}");

        #endregion

        // [Obsolete("Internal use only")]
        internal FrameWrapper(Frame frame, INavigationService navigationService)
        {
            (this as IFrameWrapperInternal).Frame = frame;
            frame.Navigating += (s, e) =>
            {
                //if (!BackStack.Any() && string.IsNullOrEmpty(FrameId))
                //{
                //    // auto-assign the FrameId if it isn't assigned
                //    FrameId = e.SourcePageType.ToString();
                //}
            };

            (this as IFrameWrapperInternal).NavigationService = navigationService;

            // setup animations
            SetupAnnimations();
        }

        private void SetupAnnimations()
        {
            var t = new NavigationThemeTransition
            {
                DefaultNavigationTransitionInfo = new EntranceNavigationTransitionInfo()
            };
            (this as IFrameWrapperInternal).Frame.ContentTransitions = new TransitionCollection { };
            (this as IFrameWrapperInternal).Frame.ContentTransitions.Add(t);
        }

        public string FrameId { get; set; } = "DefaultFrame";

        public object Content => (this as IFrameWrapperInternal).Frame.Content;

        public object GetValue(DependencyProperty dp) => (this as IFrameWrapperInternal).Frame.GetValue(dp);

        public void SetValue(DependencyProperty dp, object value) { (this as IFrameWrapperInternal).Frame.SetValue(dp, value); }

        public void ClearValue(DependencyProperty dp) { (this as IFrameWrapperInternal).Frame.ClearValue(dp); }

        public void ClearCache(bool removeCachedPagesInBackStack = false)
        {
            DebugWrite($"Frame: {FrameId}");

            var frame = (this as IFrameWrapperInternal).Frame;

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

        internal bool Navigate(Type page, object parameter) => (this as IFrameWrapperInternal).Frame.Navigate(page, parameter);

        public void PurgeNavigationState() => (this as IFrameWrapperInternal).Frame.SetNavigationState("1,0");

        public IList<PageStackEntry> BackStack => (this as IFrameWrapperInternal).Frame.BackStack;

        public IList<PageStackEntry> ForwardStack => (this as IFrameWrapperInternal).Frame.ForwardStack;

    }

    public partial class FrameWrapper : IFrameWrapperInternal
    {
        // Internal properties/methods

        string FrameStateKey => $"Frame-{FrameId}";

        async Task<FrameWrapperState> IFrameWrapperInternal.GetFrameStateAsync()
        {
            var store = await Settings.PersistedDictionaryFactory.CreateAsync(FrameStateKey);
            return new FrameWrapperState(store);
        }

        async Task<IPersistedDictionary> IFrameWrapperInternal.GetPageStateAsync(Type page)
        {
            if (page == null) return null;
            return await Settings.PersistedDictionaryFactory.CreateAsync($"Page-{page.ToString()}", FrameStateKey);
        }

        bool IFrameWrapperInternal.CanGoBack => (this as IFrameWrapperInternal).Frame.CanGoBack;

        bool IFrameWrapperInternal.CanGoForward => (this as IFrameWrapperInternal).Frame.CanGoForward;

        void IFrameWrapperInternal.SetNavigationState(string state) => (this as IFrameWrapperInternal).Frame.SetNavigationState(state);

        string IFrameWrapperInternal.GetNavigationState() => (this as IFrameWrapperInternal).Frame.GetNavigationState();

        void IFrameWrapperInternal.GoForward()
        {
            try
            {
                if ((this as IFrameWrapperInternal).CanGoForward)
                {
                    (this as IFrameWrapperInternal).Frame.GoForward();
                }
            }
            catch (Exception)
            {
                Debugger.Break();
                throw;
            }
        }

        bool IFrameWrapperInternal.Navigate(Type page, object parameter, NavigationTransitionInfo info) => (this as IFrameWrapperInternal).Frame.Navigate(page, parameter, info);

        void IFrameWrapperInternal.GoBack(NavigationTransitionInfo infoOverride)
        {
            try
            {
                if ((this as IFrameWrapperInternal).CanGoBack)
                {
                    (this as IFrameWrapperInternal).Frame.GoBack(infoOverride);
                }
            }
            catch (Exception)
            {
                Debugger.Break();
                throw;
            }
        }

        void IFrameWrapperInternal.GoBack()
        {
            try
            {
                if ((this as IFrameWrapperInternal).CanGoBack)
                {
                    (this as IFrameWrapperInternal).Frame.GoBack();
                }
            }
            catch (Exception)
            {
                Debugger.Break();
                throw;
            }
        }

        Frame IFrameWrapperInternal.Frame { get; set; }

        INavigationService IFrameWrapperInternal.NavigationService { get; set; }
    }
}
