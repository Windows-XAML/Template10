using Template10.Mvvm;

namespace MultiplePageHeaders.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public void Test() => 
            NavigationService.Navigate(typeof(Views.ContainerPage));

        public void GotoSettings() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 2);
    }
}

