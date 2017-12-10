using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Extensions;
using Windows.UI.Xaml.Controls;
using Template10.Navigation;
using Template10.Services.Dialog;
using Template10.Services.Logging;
using Template10.Services.Resources;
using Sample.Services;
using System.Windows.Input;

namespace Sample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private ILocalDialogService _dialogService;
        private IProfileRepository _profileRepository;

        public MainPageViewModel(ILocalDialogService dialog, IProfileRepository profileRepository)
        {
            _dialogService = dialog;
            _profileRepository = profileRepository;
        }

        Models.Profile _profile;
        public Models.Profile Profile
        {
            get => _profile;
            set => Set(ref _profile, value);
        }

        public async override Task OnNavigatedToAsync(INavigatedToParameters parameter)
        {
            if (parameter.NavigationMode != NavMode.Back)
            {
                if (!await TryRestoreViewModelAsync(parameter))
                {
                    Profile = new Models.Profile
                    {
                        FirstName = "Jerry",
                        LastName = "Nixon",
                        Email = "jerry.nixon@microsoft.com",
                        Web = "http://jerrynixon.com"
                    };
                }
            }
        }

        public async override Task OnNavigatedFromAsync(INavigatedFromParameters parameters)
        {
            await PersistViewModelAsync(parameters);
        }

        public async override Task<bool> CanNavigateAsync(IConfirmNavigationParameters parameters)
        {
            if (parameters.GoingTo(typeof(Views.DetailPage)))
            {
                return await _dialogService.ShowAreYouSureAsync();
            }
            else
            {
                return true;
            }
        }

        private ICommand _submitCommand;
        public ICommand SubmitCommand
        {
            get
            {
                if (_submitCommand == null)
                {
                    _submitCommand = new GalaSoft.MvvmLight.Command.RelayCommand(SubmitCommandExecute);
                }
                return _submitCommand;
            }
        }
        private async void SubmitCommandExecute()
        {
            await NavigationService.NavigateAsync(PageKeys.DetailPage.ToString());
        }

        private async Task<bool> TryRestoreViewModelAsync(INavigatedToParameters parameter)
        {
            var state = parameter.ToNavigationInfo.PageState;
            var result = await state.TryGetAsync<string>(nameof(Profile));
            if (result.Success)
            {
                Profile = await _profileRepository.LoadAsync(result.Value);
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task PersistViewModelAsync(INavigatedFromParameters parameters)
        {
            var state = parameters.PageState;
            await _profileRepository.SaveAsync(Profile);
            await state.TrySetAsync(nameof(Profile), Profile.Key);
        }
    }
}
