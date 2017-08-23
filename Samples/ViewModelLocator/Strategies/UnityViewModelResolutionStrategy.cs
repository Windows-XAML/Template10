using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Core;
using Template10.Mvvm;
using Template10.Services.Container;
using Template10.Strategies;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.Container.Unity.Demo.Strategies
{
    internal class UnityViewModelResolutionStrategy : IViewModelResolutionStrategy
    {
        public async Task<object> ResolveViewModel(Type type)
            => await Task.FromResult(ContainerService.Default.Resolve<ITemplate10ViewModel, ViewModelBase>(type.Name));

        public async Task<object> ResolveViewModel(Page page)
            => await ResolveViewModel(page.GetType());
    }
}
