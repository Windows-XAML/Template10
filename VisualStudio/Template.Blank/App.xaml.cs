using System;
using Template10.Core;
using Template10.Common;
using Template10.Extensions;
using Windows.ApplicationModel.Activation;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Template10.Navigation;

namespace Sample
{
    sealed partial class App : Template10.BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }


        public override async Task OnStartAsync(IStartArgsEx e, INavigationService navService, ISessionState sessionState)
        {
            await navService.NavigateAsync(typeof(Views.MainPage));
        }
    }
}
