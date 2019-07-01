using Prism.Navigation;
using Prism.Mvvm;
using Sample.Models;
using Sample.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Template10.Utilities;
using Prism.Events;
using Template10.SampleData.Food;

namespace Sample.ViewModels
{
    class SearchPageViewModel : BindableBase, INavigatedAware
    {
        private readonly IDataService _dataService;
        private readonly IEventAggregator _eventAggregator;
        private readonly NavigationService _navigationService;

        public SearchPageViewModel(IDataService dataService, IEventAggregator eventAggregator, NavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<string>("SearchTerm", out var searchTerm))
            {
                Search(SearchTerm = searchTerm);
            }
            else
            {
                SearchTerm = "No search term provided.";
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            // empty
        }

        void Search(string term)
        {
            Items.AddRange(_dataService.Search(term));
            HeaderText = $"Searching for '{term}'. {Items.Count} item(s) found.";
        }

        private string _headerText;
        public string HeaderText
        {
            get => _headerText;
            set => SetProperty(ref _headerText, value);
        }

        private string _searchTerm;
        public string SearchTerm
        {
            get => _searchTerm;
            set => SetProperty(ref _searchTerm, value);
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
    }
}
