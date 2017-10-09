using System.Threading.Tasks;
using Template10.Core;
using System;
using Windows.UI.Xaml.Controls;
using Template10.Strategies;
using Template10.Services.Dependency;

namespace Sample
{
    public class CustomViewModelResolutionStrategy : IViewModelResolutionStrategy
    {
        IDependencyService dependencyService = DependencyService.Default;

        public CustomViewModelResolutionStrategy()
        {
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<object> ResolveViewModelAsync(Type type)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            => dependencyService.Resolve<ITemplate10ViewModel>(type.ToString());

        public async Task<object> ResolveViewModelAsync(Page page)
            => await ResolveViewModelAsync(page.GetType());
    }
}
