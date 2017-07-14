using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.NavigationService
{
    public class DefaultViewModelResolutionStrategy : IViewModelResolutionStrategy
    {
        public Task<object> ResolveViewModel(Type type)
        {
            return null;
        }

        public Task<object> ResolveViewModel(Page page)
        {
            return null;
        }
    }
}
