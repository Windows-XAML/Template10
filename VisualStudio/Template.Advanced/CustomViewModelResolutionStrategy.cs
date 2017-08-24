using System.Threading.Tasks;
using Template10.Core;
using System;
using Windows.UI.Xaml.Controls;
using Template10.Strategies;
using Template10.Services.Container;

namespace Sample
{
    public class CustomViewModelResolutionStrategy : IViewModelResolutionStrategy
    {
        IContainerService container = ContainerService.Default;

        public async Task<object> ResolveViewModelAsync(Type type)
            => container.Resolve<ITemplate10ViewModel>(type.ToString());

        public async Task<object> ResolveViewModelAsync(Page page)
            => await ResolveViewModelAsync(page.GetType());
    }
}
