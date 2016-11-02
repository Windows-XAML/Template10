using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.BCL;
using Template10.Services.Lifetime;
using Template10.Services.Serialization;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Services.Navigation
{

    public interface IViewModelService : IService
    {
        object ResolveForPage(Page page);
    }

}