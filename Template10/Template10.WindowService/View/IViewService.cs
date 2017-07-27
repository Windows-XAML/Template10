using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Template10.Core
{
    public interface IViewService
    {
        ///<summary>
        /// Creates and opens new secondary view        
        /// </summary>
        /// <param name="frame">XAML.Frame</param>
        /// <param name="title">Title that will be displayed for new view. If <code>null</code> - current view's title will be used</param>
        /// <param name="size">Anchor size for newly created view</param>        
        /// <returns><see cref="ViewLifetimeControl"/> object that is associated to newly created view. Use it to subscribe to <code>Released</code> event to close window manually.
        /// It won't not be called before all previously started async operations on <see cref="CoreDispatcher"/> complete. <remarks>DO NOT call operations on Dispatcher after this</remarks></returns>
        Task<IViewLifetimeControl> OpenAsync(UIElement content, string title = null, ViewSizePreference size = ViewSizePreference.UseHalf);
    }
}