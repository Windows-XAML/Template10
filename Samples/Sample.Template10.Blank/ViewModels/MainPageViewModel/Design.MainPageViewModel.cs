using Prism.Mvvm;

namespace Sample.ViewModels.Design
{
    public class MainPageViewModel : BindableBase, IMainPageViewModel
    {
        public MainPageViewModel()
        {
            Title = "Hello design-time world.";
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
