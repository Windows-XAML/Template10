using System;
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
    public class NavigatedLogic : INavigatedLogic
    {
        public static INavigatedLogic Instance { get; set; } = new NavigatedLogic();
        private NavigatedLogic()
        {
            // private constructor
        }

        public async virtual Task CallNavigatedToAsync(INavigatedAware vm, INavigationParameter parameter, NavigationModes mode)
        {
            this.LogInfo();

            if (vm == null)
            {
                return;
            }

            await vm.OnNavigatedToAsync(parameter, mode);
        }


        public async virtual Task CallNavigatedFromAsync(INavigatedAware vm)
        {
            this.LogInfo();

            if (vm != null)
            {
                await vm.OnNavigatedFromAsync();
            }
        }
    }
}
