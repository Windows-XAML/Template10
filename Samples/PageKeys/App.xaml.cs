using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Template10.Samples.PageKeysSample
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public enum Pages { MainPage, DetailPage, AboutPage }

        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            var keys = PageKeys<Pages>();
            if (!keys.ContainsKey(Pages.MainPage))
                keys.Add(Pages.MainPage, typeof(Views.MainPage));
            if (!keys.ContainsKey(Pages.DetailPage))
                keys.Add(Pages.DetailPage, null); // TODO
            if (!keys.ContainsKey(Pages.AboutPage))
                keys.Add(Pages.AboutPage, null); // TODO
            return Task.CompletedTask;
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(Pages.MainPage, 2);
            return Task.CompletedTask;
        }
    }
}
