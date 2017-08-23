using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Template10.Strategies
{
    public class DefaultViewModelResolutionStrategy : IViewModelResolutionStrategy
    {
        public async Task<object> ResolveViewModelAsync(Type type)
        {
            return null;
        }

        public async Task<object> ResolveViewModelAsync(Page page)
        {
            return null;
        }
    }
}
