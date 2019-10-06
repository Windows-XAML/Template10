using Template10.Mvvm;
using Template10.Navigation;

namespace Template10.NuGet.FileActivationSample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Title = "The application was launched normally.";
        }
    }
}
