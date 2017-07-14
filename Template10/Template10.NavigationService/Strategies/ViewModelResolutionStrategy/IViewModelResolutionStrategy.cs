using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.NavigationService
{
    public interface IViewModelResolutionStrategy
    {
        Task<object> ResolveViewModel(Type type);
        Task<object> ResolveViewModel(Page page);
    }
}
