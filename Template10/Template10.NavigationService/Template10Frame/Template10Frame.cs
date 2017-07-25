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
    public partial class Template10Frame : ITemplate10FrameInternal, ITemplate10Frame
    {
        #region Debug

        static void DebugWrite(string text = null, Services.LoggingService.Severities severity = LoggingService.Severities.Template10, [CallerMemberName]string caller = null) =>
            LoggingService.LoggingService.WriteLine(text, severity, caller: $"{nameof(Template10Frame)}.{caller}");

        #endregion

        // [Obsolete("Internal use only")]
        internal Template10Frame(Frame frame, INavigationService navigationService)
        {
            (this as ITemplate10FrameInternal).Frame = frame;
            frame.Navigating += (s, e) =>
            {
                //if (!BackStack.Any() && string.IsNullOrEmpty(FrameId))
                //{
                //    // auto-assign the FrameId if it isn't assigned
                //    FrameId = e.SourcePageType.ToString();
                //}
            };

            (this as ITemplate10FrameInternal).NavigationService = navigationService;

            // setup animations
            SetupAnnimations();
        }

        private void SetupAnnimations()
        {
            var t = new NavigationThemeTransition
            {
                DefaultNavigationTransitionInfo = new EntranceNavigationTransitionInfo()
            };
            (this as ITemplate10FrameInternal).Frame.ContentTransitions = new TransitionCollection { };
            (this as ITemplate10FrameInternal).Frame.ContentTransitions.Add(t);
        }

        public string FrameId { get; set; } = "DefaultFrame";

        public object Content => (this as ITemplate10FrameInternal).Frame.Content;

        public object GetValue(DependencyProperty dp) => (this as ITemplate10FrameInternal).Frame.GetValue(dp);

        public void SetValue(DependencyProperty dp, object value) { (this as ITemplate10FrameInternal).Frame.SetValue(dp, value); }

        public void ClearValue(DependencyProperty dp) { (this as ITemplate10FrameInternal).Frame.ClearValue(dp); }

        public void ClearCache(bool removeCachedPagesInBackStack = false)
        {
            DebugWrite($"Frame: {FrameId}");

            var frame = (this as ITemplate10FrameInternal).Frame;

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

        internal bool Navigate(Type page, object parameter) => (this as ITemplate10FrameInternal).Frame.Navigate(page, parameter);

        public void PurgeNavigationState() => (this as ITemplate10FrameInternal).Frame.SetNavigationState("1,0");

        public IList<PageStackEntry> BackStack => (this as ITemplate10FrameInternal).Frame.BackStack;

        public IList<PageStackEntry> ForwardStack => (this as ITemplate10FrameInternal).Frame.ForwardStack;

    }

    public partial class Template10Frame : ITemplate10FrameInternal
    {
        // Internal properties/methods

        string FrameStateKey => $"Frame-{FrameId}";

        async Task<Template10FrameState> ITemplate10FrameInternal.GetFrameStateAsync()
        {
            var store = await Settings.PersistedDictionaryFactory.CreateAsync(FrameStateKey);
            return new Template10FrameState(store);
        }

        async Task<IPersistedDictionary> ITemplate10FrameInternal.GetPageStateAsync(Type page)
        {
            if (page == null) return null;
            return await Settings.PersistedDictionaryFactory.CreateAsync($"Page-{page.ToString()}", FrameStateKey);
        }

        async Task<IPersistedDictionary> ITemplate10FrameInternal.GetPageStateAsync(string page)
        {
            if (string.IsNullOrEmpty(page)) return null;
            return await Settings.PersistedDictionaryFactory.CreateAsync($"Page-{page}", FrameStateKey);
        }

        bool ITemplate10FrameInternal.CanGoBack => (this as ITemplate10FrameInternal).Frame.CanGoBack;

        bool ITemplate10FrameInternal.CanGoForward => (this as ITemplate10FrameInternal).Frame.CanGoForward;

        void ITemplate10FrameInternal.SetNavigationState(string state) => (this as ITemplate10FrameInternal).Frame.SetNavigationState(state);

        string ITemplate10FrameInternal.GetNavigationState() => (this as ITemplate10FrameInternal).Frame.GetNavigationState();

        void ITemplate10FrameInternal.GoForward()
        {
            try
            {
                if ((this as ITemplate10FrameInternal).CanGoForward)
                {
                    (this as ITemplate10FrameInternal).Frame.GoForward();
                }
            }
            catch (Exception)
            {
                Debugger.Break();
                throw;
            }
        }

        bool ITemplate10FrameInternal.Navigate(Type page, object parameter, NavigationTransitionInfo info) => (this as ITemplate10FrameInternal).Frame.Navigate(page, parameter, info);

        void ITemplate10FrameInternal.GoBack(NavigationTransitionInfo infoOverride)
        {
            try
            {
                if ((this as ITemplate10FrameInternal).CanGoBack)
                {
                    (this as ITemplate10FrameInternal).Frame.GoBack(infoOverride);
                }
            }
            catch (Exception)
            {
                Debugger.Break();
                throw;
            }
        }

        void ITemplate10FrameInternal.GoBack()
        {
            try
            {
                if ((this as ITemplate10FrameInternal).CanGoBack)
                {
                    (this as ITemplate10FrameInternal).Frame.GoBack();
                }
            }
            catch (Exception)
            {
                Debugger.Break();
                throw;
            }
        }

        Frame ITemplate10FrameInternal.Frame { get; set; }

        INavigationService ITemplate10FrameInternal.NavigationService { get; set; }
    }
}
