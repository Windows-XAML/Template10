using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Template10.Strategies
{
    public class DefaultViewModelResolutionStrategy : IViewModelResolutionStrategy
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        public async Task<object> ResolveViewModelAsync(Type type)
        {
            return null;
        }

        public async Task<object> ResolveViewModelAsync(Page page)
        {
            return null;
        }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
