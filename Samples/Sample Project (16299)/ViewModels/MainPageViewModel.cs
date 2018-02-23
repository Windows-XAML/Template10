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
using Template10.Utilities;

namespace Sample.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private NavigationService _navigationService;

        public MainPageViewModel(IDataService dataService, IDialogService dialogService)
        {
            _dataService = dataService;
            _dialogService = dialogService;
        }

        private string _headerText;
        public string HeaderText
        {
            get => _headerText;
            set => SetProperty(ref _headerText, value);
        }

        private DataItem _selectedItem;
        public DataItem SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public ObservableCollection<DataItem> Items { get; } = new ObservableCollection<DataItem>();
        public ObservableCollection<DataGroup> Groups { get; } = new ObservableCollection<DataGroup>();

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            _navigationService = parameters.GetNavigationService();
            LoadData();
        }

        public override bool CanNavigate(INavigationParameters parameters)
        {
            return IsValid();
        }

        private void LoadData()
        {
            Items.AddRange(_dataService.GetItems(150), true);
            Groups.AddRange(_dataService.GetGroups(10, 10));
        }

        private bool SaveData()
        {
            return true;
        }

        private bool IsValid()
        {
            return true;
        }
    }
}
