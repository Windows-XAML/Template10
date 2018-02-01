using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace PrismSample.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
        NavigationService NavigationService { get; set; }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            NavigationService = parameters.GetNavigationService();
            await LoadData();
        }

        public override async Task<bool> CanNavigateAsync(INavigationParameters parameters)
        {
            if (await Validate())
            {
                return await SaveData();
            }
            return false;
        }

        private string _value;
        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public async void Go()
        {
            var path = $"/{nameof(Views.MainPage)}?Value={Value}";
            await NavigationService.NavigateAsync(path);
        }

        private async Task LoadData()
        {
            await Task.CompletedTask;
        }

        private async Task<bool> Validate()
        {
            await Task.CompletedTask;
            return true;
        }

        private async Task<bool> SaveData()
        {
            await Task.CompletedTask;
            return true;
        }
    }
}
