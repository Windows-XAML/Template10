using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Core;
using Template10.Extensions;
using Template10.Services.Logging;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Template10.Navigation
{
    public partial class FrameEx : IFrameEx
    {
        internal static IFrameEx Create(Frame frame, INavigationService navigationService)
        {
            return new FrameEx(frame, navigationService);
        }

        IFrameEx2 Two => this as IFrameEx2;

        Strategies.INavStateStrategy IFrameEx2.StateStrategy { get; set; } = new Strategies.DefaultNavStateStrategy();


        internal FrameEx(Frame frame, INavigationService navigationService)
        {
            Two.Frame = frame;
            Two.NavigationService = navigationService;
            SetupAnimations();
        }

        private void SetupAnimations()
        {
            var t = new NavigationThemeTransition
            {
                DefaultNavigationTransitionInfo = new EntranceNavigationTransitionInfo()
            };
            Two.Frame.ContentTransitions = new TransitionCollection { };
            Two.Frame.ContentTransitions.Add(t);
        }

        public string FrameId { get; set; } = "DefaultFrame";

        public ElementTheme RequestedTheme
        {
            get => Two.Frame.RequestedTheme;
            set => Two.Frame.RequestedTheme = value;
        }

        public object Content => Two.Frame.Content;

        public object GetValue(DependencyProperty dp) => Two.Frame.GetValue(dp);

        public void SetValue(DependencyProperty dp, object value) { Two.Frame.SetValue(dp, value); }

        public void ClearValue(DependencyProperty dp) { Two.Frame.ClearValue(dp); }

        public void ClearNavigationCache(bool removeCachedPagesInBackStack = false)
        {
            this.Log($"Frame: {FrameId}");

            var frame = Two.Frame;

            var currentSize = frame.CacheSize;
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

        internal bool Navigate(Type page, object parameter)
        {
            parameter = SerializeParameter(parameter);
            return Two.Frame.Navigate(page, parameter);
        }

        private object SerializeParameter(object parameter)
        {
            if (Settings.RequireSerializableParameters)
            {
                if (Central.Serialization.TrySerialize(parameter, out var serialized_parameter))
                {
                    this.Log($"Parameter serialized by Settings: {serialized_parameter}");
                    return serialized_parameter;
                }
                else
                {
                    throw new InvalidOperationException("Navigation settings require serializable parameter.");
                }
            }
            else
            {
                this.Log($"Parameter not serialized by Settings.");
                return parameter;
            }
        }

        public void PurgeNavigationState() => Two.Frame.SetNavigationState("1,0");

        public IList<PageStackEntry> BackStack => Two.Frame.BackStack;

        public IList<PageStackEntry> ForwardStack => Two.Frame.ForwardStack;

    }

    public partial class FrameEx : IFrameEx2
    {
        // Internal properties/methods

        string FrameStateKey => $"Frame-{FrameId}";

        async Task<FrameExState> IFrameEx2.GetFrameStateAsync() => await Two.StateStrategy.GetFrameStateAsync(FrameId);

        async Task<IPropertyBagEx> IFrameEx2.GetPageStateAsync(Type page) => await Two.GetPageStateAsync(page?.ToString());

        async Task<IPropertyBagEx> IFrameEx2.GetPageStateAsync(string page)
        {
            if (string.IsNullOrEmpty(page))
            {
                return null;
            }
            return await Two.StateStrategy.GetPageStateAsync(FrameId, page);
        }

        bool IFrameEx2.CanGoBack => Two.Frame.CanGoBack;

        bool IFrameEx2.CanGoForward => Two.Frame.CanGoForward;

        void IFrameEx2.SetNavigationState(string state) => Two.Frame.SetNavigationState(state);

        string IFrameEx2.GetNavigationState() => Two.Frame.GetNavigationState();

        void IFrameEx2.GoForward()
        {
            try
            {
                if (Two.CanGoForward)
                {
                    Two.Frame.GoForward();
                }
            }
            catch (Exception)
            {
                Debugger.Break();
                throw;
            }
        }

        bool IFrameEx2.Navigate(Type page, object parameter, NavigationTransitionInfo info) => Two.Frame.Navigate(page, parameter, info);

        void IFrameEx2.GoBack(NavigationTransitionInfo infoOverride)
        {
            try
            {
                if (Two.CanGoBack)
                {
                    Two.Frame.GoBack(infoOverride);
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
                if (Two.CanGoBack)
                {
                    Two.Frame.GoBack();
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
