using System;
using System.Threading.Tasks;
using Template10.Utils;
using Windows.ApplicationModel.Activation;

namespace Template10.Samples.PageKeysSample
{
    sealed partial class App : Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public enum Pages
        {
            MainPage,
            DetailPage,
            AboutPage
        }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            var keys = PageKeys<Pages>();
            keys.TryAdd(Pages.MainPage, typeof(Views.MainPage));
            keys.TryAdd(Pages.DetailPage, null);
            keys.TryAdd(Pages.AboutPage, null);
            await Task.CompletedTask;
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            await NavigationService.NavigateAsync(Pages.MainPage, 2);
        }
    }
}
