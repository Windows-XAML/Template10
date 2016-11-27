using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.BCL;
using Template10.Services.Lifetime;
using Template10.Services.Serialization;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Services.Navigation
{

    public class NavigatingLogic : INavigatingLogic
    {
        public static INavigatingLogic Instance { get; set; } = new NavigatingLogic();
        private NavigatingLogic()
        {
            // private constructor
        }

        public async virtual Task CallNavigatingToAsync(INavigatingAware vm, INavigationParameter parameter, NavigationModes mode)
        {
            this.LogInfo();

            if (vm == null)
            {
                return;
            }

            await vm.OnNavigatingToAsync(parameter, mode);
        }

        public async virtual Task CallNavigatingFromAsync(INavigatingAware vm)
        {
            this.LogInfo();

            if (vm != null)
            {
                await vm.OnNavigatingFromAsync();
            }
        }
    }

}