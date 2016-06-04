using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Template10.Samples.PageKeysSample
{
    sealed partial class App : Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public enum Pages { MainPage, DetailPage, AboutPage }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            var keys = PageKeys<Pages>();
            var add = new Action<Pages, Type>((p, t) =>
            {
                if (!keys.ContainsKey(p))
                {
                    keys.Add(p, t);
                }
            });
            add(Pages.MainPage, null);
            add(Pages.DetailPage, null);
            add(Pages.AboutPage, null);
            await Task.CompletedTask;
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            await NavigationService.NavigateAsync(Pages.MainPage, 2);
        }
    }
}
