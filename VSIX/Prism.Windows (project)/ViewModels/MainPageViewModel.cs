using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using PrismSample.Services;
using PrismSample.Models;
using System.Collections.ObjectModel;

namespace PrismSample.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private NavigationService _navigationService;

        public MainPageViewModel(IDataService dataService)
        {
            _dataService = dataService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            _navigationService = parameters.GetNavigationService();
            LoadData();
        }

        public override bool CanNavigate(INavigationParameters parameters)
        {
            if (IsValidate())
            {
                return SaveData();
            }
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
