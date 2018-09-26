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
using System.Diagnostics;
using Prism.Events;

namespace Sample.ViewModels
{
    class MainPageViewModel : BindableBase, INavigatedAware, IConfirmNavigation
    {
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private NavigationService _navigationService;

        public MainPageViewModel(IDataService dataService, IDialogService dialogService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
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
            set
            {
                SetProperty(ref _selectedItem, value);
                if (value != null)
                {
                    _eventAggregator.GetEvent<Messages.ShowEditorMessage>().Publish(value);
                }
            }
        }

        public ObservableCollection<DataItem> Items { get; } = new ObservableCollection<DataItem>();
        public ObservableCollection<DataGroup> Groups { get; } = new ObservableCollection<DataGroup>();

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<int>("Record", out var record))
            {
                Debug.WriteLine(record);
            }
            _navigationService = parameters.GetNavigationService();
            LoadData();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            // empty
        }

        public bool CanNavigate(INavigationParameters parameters)
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
