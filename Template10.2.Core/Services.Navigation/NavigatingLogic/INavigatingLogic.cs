using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.BCL;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.Navigation
{

    public interface INavigatingLogic : ILogic
    {
        Task CallNavigatingToAsync(INavigatingAware vm, INavigationParameter parameter, NavigationModes mode);
        Task CallNavigatingFromAsync(INavigatingAware vm);
    }

}