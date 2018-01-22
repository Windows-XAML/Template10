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
            if (parameter.NavigationMode != NavMode.Back && !await TryLoadViewModelAsync(parameter))
            {
                CreateDefaultProfile();
            }
        }

        public async override Task OnNavigatedFromAsync(INavigatedFromParameters parameters)
        {
            await SaveViewModelAsync(parameters);
        }

        public async override Task<bool> CanNavigateAsync(IConfirmNavigationParameters parameters)
        {
            if (parameters.GoingTo(typeof(Views.DetailPage)))
            {
                return await _dialogService.ShowAreYouSureAsync();
            }
            return true;
        }

        private ICommand _submitCommand;
        public ICommand SubmitCommand
        {
            get
            {
                if (_submitCommand == null)
                {
                    _submitCommand = new GalaSoft.MvvmLight.Command.RelayCommand(async () =>
                    {
                        await NavigationService.NavigateAsync(PageKeys.DetailPage.ToString());
                    }, () =>
                    {
                        return true;
                    });
                }
                return _submitCommand;
            }
        }

        private async Task<bool> TryLoadViewModelAsync(INavigatedToParameters parameter)
        {
            var state = parameter.ToNavigationInfo.PageState;
            var result = await state.TryGetAsync<string>(nameof(Profile));
            if (result.Success)
            {
                Profile = await _profileRepository.LoadAsync(result.Value);
                return true;
            }
            return false;
        }

        private async Task SaveViewModelAsync(INavigatedFromParameters parameters)
        {
            var state = parameters.PageState;
            await _profileRepository.SaveAsync(Profile);
            await state.TrySetAsync(nameof(Profile), Profile.Key);
        }

        private void CreateDefaultProfile()
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
