using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Sample.Services;
using Sample.Models;
using System.Collections.ObjectModel;
using System.Threading;
using Template10.Services.Dialog;

namespace Sample.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private NavigationService _navigationService;
        private SynchronizationContext _syncContext;

        public MainPageViewModel(IDataService dataService, IDialogService dialogService)
        {
            _dataService = dataService;
            _dialogService = dialogService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            _navigationService = parameters.GetNavigationService();
            LoadData();
        }

        public override async Task<bool> CanNavigateAsync(INavigationParameters parameters)
        {
            //if (await _dialogService.PromptAsync("Are you sure?") == MessageBoxResult.Yes)
            //{
            if (IsValidate())
            {
                return SaveData();
            }
            //}
            return false;
        }

        private DataItem _selectedItem;
        public DataItem SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public ObservableCollection<DataItem> Items { get; } = new ObservableCollection<DataItem>();

        private void LoadData()
        {
            Items.Clear();
            foreach (var item in _dataService.GetItems())
            {
                Items.Add(item);
            }
        }

        private bool SaveData()
        {
            return true;
        }

        private bool IsValidate()
        {
            return true;
        }
    }
}
