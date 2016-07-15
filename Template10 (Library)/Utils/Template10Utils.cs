using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Utils
{
    public static class Template10Utils
    {
        public static INavigationService GetNavigationService(this Frame frame)
            => NavigationService.GetForFrame(frame);

        public static async Task<bool> NavigateAsyncEx(this Frame frame, Type page, object parameter = null, NavigationTransitionInfo infoOverride = null)
            => await frame.GetNavigationService().NavigateAsync(page, parameter, infoOverride);

        public static async Task<bool> NavigateAsyncEx<T>(this Frame frame, T key, object parameter = null, NavigationTransitionInfo infoOverride = null) where T : struct, IConvertible
            => await frame.GetNavigationService().NavigateAsync(key, parameter, infoOverride);

        public static WindowWrapper GetWindowWrapper(this INavigationService service)
            => WindowWrapper.ActiveWrappers.FirstOrDefault(x => x.NavigationServices.Contains(service));

        public static IDispatcherWrapper GetDispatcherWrapper(this INavigationService service)
            => service.GetWindowWrapper().Dispatcher;

        public static IDispatcherWrapper GetDispatcherWrapper(this CoreDispatcher wrapper) 
            => new DispatcherWrapper(wrapper);

        /// <summary>
        /// Returns a list of submenu buttons with the same GroupName attribute as the command button upon which this
        /// extension is invoked (which is treated as Parent command button).
        /// </summary>
        /// <returns>Submenu buttons in List&lt;HamburgerButtonInfo&gt;. If no submenu buttons found,  List is still returned with element count of 0. </returns>
        /// <remarks>
        /// For added convenience, the GroupName attribute is detected with string.StartWith(groupName) rather than
        /// the straightforward string.Equals(groupName). That way we can tag submenu buttons as groupName1, groupName2, 
        /// groupName3, etc. With this scheme, the parent command button should be named by subset string, 
        /// which in this case is groupName.
        /// You don't have to use this scheme in which case you just stick to a single groupName for all buttons.
        /// </remarks>

        public static List<HamburgerButtonInfo> ItemsInGroup(this HamburgerButtonInfo button, bool IncludeSecondaryButtons = false)
        {
            string groupName = button.GroupName?.ToString();

            // Return 0 count List rather than null
            if (string.IsNullOrWhiteSpace(groupName)) return new List<HamburgerButtonInfo>();

            FrameworkElement fe = button.Content as FrameworkElement;
            HamburgerMenu hamMenu = fe.FirstAncestor<HamburgerMenu>();

            List<HamburgerButtonInfo> NavButtons = hamMenu.PrimaryButtons.ToList();
            if (IncludeSecondaryButtons) NavButtons.InsertRange(NavButtons.Count, hamMenu.SecondaryButtons.ToList());

            List<HamburgerButtonInfo> groupItems = NavButtons.Where(x => !x.Equals(button) && (x.GroupName?.ToString()?.StartsWith(groupName) ?? false)).ToList();

            return groupItems;
        }
    }
}
