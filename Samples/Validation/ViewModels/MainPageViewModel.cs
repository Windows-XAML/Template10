using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Utils;
using Windows.UI.Xaml.Navigation;

namespace Template10.Samples.ValidationSample.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
        Services.UserService.UserService _userService;

        public MainPageViewModel()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _userService = new Services.UserService.UserService();
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Users.AddRange(_userService.GetUsers());

            var refresh = new Action(() =>
            {
                DeleteSelectedCommand.RaiseCanExecuteChanged();
                CreateAndSelectCommand.RaiseCanExecuteChanged();
            });
            PropertyChanged += (s, e) => refresh();
            Users.CollectionChanged += (s, e) => refresh();
            return Task.CompletedTask;
        }

        public ObservableCollection<Models.User> Users { get; } = new ObservableCollection<Models.User>();

        Models.User _Selected = default(Models.User);
        public Models.User Selected { get { return _Selected; } set { Set(ref _Selected, value); } }

        DelegateCommand _DeleteSelectedCommand;
        public DelegateCommand DeleteSelectedCommand => _DeleteSelectedCommand ?? (_DeleteSelectedCommand = new DelegateCommand(DeleteSelectedExecute, DeleteSelectedCanExecute));
        bool DeleteSelectedCanExecute() => !(Selected?.IsAdmin ?? true);
        void DeleteSelectedExecute()
        {
            _userService.DeleteUsers(Selected.Id);
            Users.Remove(Selected);
        }

        DelegateCommand _CreateAndSelectCommand;
        public DelegateCommand CreateAndSelectCommand => _CreateAndSelectCommand ?? (_CreateAndSelectCommand = new DelegateCommand(CreateAndSelectExecute, CreateAndSelectCanExecute));
        bool CreateAndSelectCanExecute() => !Users.Any(x => x.LastName.Contains("Shirt"));
        void CreateAndSelectExecute()
        {
            Users.Add(Selected = _userService.CreateUser());
        }
    }
}
