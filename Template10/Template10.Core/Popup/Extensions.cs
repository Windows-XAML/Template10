using System;
using System.Linq;
using Template10.Core;
using Template10.Popup;

namespace Template10.Extensions
{
    public static class PopupExtensions
    {
        public static bool TryGetBusyPopup(this IBootStrapperPopup b, out BusyPopup popup)
            => TryGetPopupItem(b, out popup);

        public static bool TryGetPopup<T>(this IBootStrapperPopup b, out T popup) where T : IPopupItem
            => TryGetPopupItem<T>(b, out popup);

        public static bool TryGetPopup<T>(out T popup) where T : IPopupItem
            => TryGetPopupItem(Central.Container.Resolve<IBootStrapperPopup>(), out popup);

        private static bool TryGetPopupItem<T>(IBootStrapperPopup b, out T popup) where T : IPopupItem
        {
            try
            {
                popup = (T)b.Popups.Single(x => x is T);
                return popup != null;
            }
            catch (Exception)
            {
                popup = default(T);
                return false;
            }
        }
    }
}
