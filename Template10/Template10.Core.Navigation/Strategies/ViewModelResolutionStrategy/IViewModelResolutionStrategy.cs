using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Template10.Strategies
{
    public interface IViewModelResolutionStrategy
    {
        Task<object> ResolveViewModelAsync(Type type);
        Task<object> ResolveViewModelAsync(Page page);
    }
}
