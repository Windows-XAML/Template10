using System.Collections.Generic;
using System.Collections.ObjectModel;
using Template10.Mvvm;
using Template10.Utils;
using Windows.UI.Xaml.Navigation;

namespace Sample.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
        Services.UserService.UserService _userService;

        public MainPageViewModel()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _userService = new Services.UserService.UserService();
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Users.AddRange(_userService.GetUsers());
        }

        public ObservableCollection<Models.User> Users { get; } = new ObservableCollection<Models.User>();

        Models.User _Selected = default(Models.User);
        public Models.User Selected { get { return _Selected; } set { Set(ref _Selected, value); } }
    }
}
