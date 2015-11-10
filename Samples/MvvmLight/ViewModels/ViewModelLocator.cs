using Windows.UI.Xaml;

namespace Sample.ViewModels
{
    public class ViewModelLocator
    {
        private ViewModels.MainPageViewModel _MainPageViewModel;
        public ViewModels.MainPageViewModel MainPageViewModel
        {
            get
            {
                if (_MainPageViewModel != null)
                    return _MainPageViewModel;
                return _MainPageViewModel = new MainPageViewModel();
            }
        }
    }
}
