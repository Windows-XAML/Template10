using System;
using System.Linq;
using Template10.Common;
using Template10.Popups;

namespace Template10.Extensions
{
    public static class PopupsExtensions
    {
        public static bool TryGetPopup<T>(this object item, out T popup) where T : IPopupItem
            => TryGetPopup<T>(out popup);

        public static bool TryGetPopup<T>(out T popup) where T : IPopupItem
            => TryGetPopupItem(Central.DependencyService.Resolve<IBootStrapperPopup>(), out popup);

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
