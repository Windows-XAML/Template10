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
using Template10.SampleData.Food;

namespace Sample.ViewModels
{
    class MainPageViewModel : BindableBase, INavigatedAware, IConfirmNavigation
    {
        private readonly IDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly NavigationService _navigationService;

        public MainPageViewModel(IDataService dataService, IDialogService dialogService, IEventAggregator eventAggregator, NavigationService navigationService)
        {
            _dataService = dataService;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _navigationService = navigationService;
        }

        private string _headerText;
        public string HeaderText
        {
            get => _headerText;
            set => SetProperty(ref _headerText, value);
        }

        private Fruit _selectedItem;
        public Fruit SelectedItem
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

        public ObservableCollection<Fruit> Items { get; } = new ObservableCollection<Fruit>();

        public ObservableCollection<Group<Fruit>> Groups { get; } = new ObservableCollection<Group<Fruit>>();

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<int>("Record", out var record))
            {
                Debug.WriteLine(record);
            }
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
            Items.AddRange(_dataService.GetItems(), true);
            Groups.AddRange(_dataService.GetGroups());
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
