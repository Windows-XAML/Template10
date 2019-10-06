using Template10.Mvvm;
using Template10.Navigation;

namespace Template10.Nuget.Sample.ViewModels
{
    public class MainPageViewModel : ViewModelBase, IMainPageViewModel
    {
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Title = "Hello run-time world.";
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            // empty
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}