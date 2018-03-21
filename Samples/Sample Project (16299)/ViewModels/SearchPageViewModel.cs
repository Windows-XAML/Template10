using Prism.Navigation;
using Prism.Mvvm;
using Sample.Models;
using Sample.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Template10.Utilities;

namespace Sample.ViewModels
{
    class SearchPageViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private NavigationService _navigationService;

        public SearchPageViewModel(IDataService dataService)
        {
            _dataService = dataService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            _navigationService = parameters.GetNavigationService();
            if (parameters.TryGetValue<string>("SearchTerm", out var searchTerm))
            {
                Search(SearchTerm = searchTerm);
            }
            else
            {
                SearchTerm = "No search term provided.";
            }

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

        public ObservableCollection<DataItem> Items { get; } = new ObservableCollection<DataItem>();
    }
}
