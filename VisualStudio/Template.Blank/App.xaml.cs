using System;
using Template10.Core;
using Template10.Common;
using Template10.Core;
using Template10.Extensions;
using Windows.ApplicationModel.Activation;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Sample
{
    sealed partial class App : Template10.BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public override async Task OnStartAsync(IStartArgsEx e)
        {
            await NavigationService.NavigateAsync(typeof(Views.MainPage));
        }
    }
}
