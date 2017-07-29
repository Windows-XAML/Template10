using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Common.PersistedDictionary;
using Template10.Portable;
using Template10.Services.LoggingService;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Navigation
{
    public partial class FrameEx : IFrameEx2, IFrameEx
    {
        #region Debug

        static void DebugWrite(string text = null, Severities severity = Severities.Template10, [CallerMemberName]string caller = null) =>
            LoggingService.WriteLine(text, severity, caller: $"{nameof(FrameEx)}.{caller}");

        #endregion

        internal static IFrameEx2 Create(Frame frame, INavigationService navigationService)
        {
            return new FrameEx(frame, navigationService);
        }

        // [Obsolete("Internal use only")]
        internal FrameEx(Frame frame, INavigationService navigationService)
        {
            (this as IFrameEx2).Frame = frame;
            frame.Navigating += (s, e) =>
            {
                //if (!BackStack.Any() && string.IsNullOrEmpty(FrameId))
                //{
                //    // auto-assign the FrameId if it isn't assigned
                //    FrameId = e.SourcePageType.ToString();
                //}
            };

            (this as IFrameEx2).NavigationService = navigationService;

            // setup animations
            SetupAnnimations();
        }

        private void SetupAnnimations()
        {
            var t = new NavigationThemeTransition
            {
                DefaultNavigationTransitionInfo = new EntranceNavigationTransitionInfo()
            };
            (this as IFrameEx2).Frame.ContentTransitions = new TransitionCollection { };
            (this as IFrameEx2).Frame.ContentTransitions.Add(t);
        }

        public string FrameId { get; set; } = "DefaultFrame";

        public object Content => (this as IFrameEx2).Frame.Content;

        public object GetValue(DependencyProperty dp) => (this as IFrameEx2).Frame.GetValue(dp);

        public void SetValue(DependencyProperty dp, object value) { (this as IFrameEx2).Frame.SetValue(dp, value); }

        public void ClearValue(DependencyProperty dp) { (this as IFrameEx2).Frame.ClearValue(dp); }

        public void ClearCache(bool removeCachedPagesInBackStack = false)
        {
            DebugWrite($"Frame: {FrameId}");

            var frame = (this as IFrameEx2).Frame;

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

        internal bool Navigate(Type page, object parameter) => (this as IFrameEx2).Frame.Navigate(page, parameter);

        public void PurgeNavigationState() => (this as IFrameEx2).Frame.SetNavigationState("1,0");

        public IList<PageStackEntry> BackStack => (this as IFrameEx2).Frame.BackStack;

        public IList<PageStackEntry> ForwardStack => (this as IFrameEx2).Frame.ForwardStack;

    }

    public partial class FrameEx : IFrameEx2
    {
        // Internal properties/methods

        string FrameStateKey => $"Frame-{FrameId}";

        async Task<FrameExState> IFrameEx2.GetFrameStateAsync()
        {
            var store = await Settings.PersistedDictionaryFactory.CreateAsync(FrameStateKey);
            return new FrameExState(store);
        }

        async Task<IPersistedDictionary> IFrameEx2.GetPageStateAsync(Type page)
        {
            if (page == null) return null;
            return await Settings.PersistedDictionaryFactory.CreateAsync($"Page-{page.ToString()}", FrameStateKey);
        }

        async Task<IPersistedDictionary> IFrameEx2.GetPageStateAsync(string page)
        {
            if (string.IsNullOrEmpty(page)) return null;
            return await Settings.PersistedDictionaryFactory.CreateAsync($"Page-{page}", FrameStateKey);
        }

        bool IFrameEx2.CanGoBack => (this as IFrameEx2).Frame.CanGoBack;

        bool IFrameEx2.CanGoForward => (this as IFrameEx2).Frame.CanGoForward;

        void IFrameEx2.SetNavigationState(string state) => (this as IFrameEx2).Frame.SetNavigationState(state);

        string IFrameEx2.GetNavigationState() => (this as IFrameEx2).Frame.GetNavigationState();

        void IFrameEx2.GoForward()
        {
            try
            {
                if ((this as IFrameEx2).CanGoForward)
                {
                    (this as IFrameEx2).Frame.GoForward();
                }
            }
            catch (Exception)
            {
                Debugger.Break();
                throw;
            }
        }

        bool IFrameEx2.Navigate(Type page, object parameter, NavigationTransitionInfo info) => (this as IFrameEx2).Frame.Navigate(page, parameter, info);

        void IFrameEx2.GoBack(NavigationTransitionInfo infoOverride)
        {
            try
            {
                if ((this as IFrameEx2).CanGoBack)
                {
                    (this as IFrameEx2).Frame.GoBack(infoOverride);
                }
            }
            catch (Exception)
            {
                Debugger.Break();
                throw;
            }
        }

        void IFrameEx2.GoBack()
        {
            try
            {
                if ((this as IFrameEx2).CanGoBack)
                {
                    (this as IFrameEx2).Frame.GoBack();
                }
            }
            catch (Exception)
            {
                Debugger.Break();
                throw;
            }
        }

        Frame IFrameEx2.Frame { get; set; }

        INavigationService IFrameEx2.NavigationService { get; set; }
    }
}
