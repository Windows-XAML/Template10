using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace Template10.Services.WindowService
{
    public interface IWindowService
    {
        Task ShowAsync<T>(object param = null, ViewSizePreference size = ViewSizePreference.UseHalf);
    }
}
