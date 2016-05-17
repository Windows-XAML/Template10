using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Utils;
using Windows.UI.Xaml.Navigation;

namespace Sample.ViewModels
{
    public class SearchPageViewModel : ViewModelBase
    {
        Services.DataService.DataService _DataService;

        public SearchPageViewModel()
        {
            _DataService = new Services.DataService.DataService();
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Filter = "Designtime filter";
                Items.AddRange(_DataService.LoadAsync().Result);
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (parameter != null)
            {
                Filter = parameter.ToString();
            }
            else if (suspensionState.Any())
            {
                Filter = suspensionState[nameof(Filter)]?.ToString();
            }
            await Task.CompletedTask;
            ApplyFilter();
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            if (suspending)
            {
                suspensionState[nameof(Filter)] = Filter;
            }
            await Task.CompletedTask;
        }

        public ObservableCollection<Models.DataItem> Items { get; } = new ObservableCollection<Models.DataItem>();

        string _Filter = string.Empty;
        public string Filter { get { return _Filter; } set { Set(ref _Filter, value); } }
        public async void ApplyFilter()
        {
            var items = await _DataService.LoadAsync();
            var filter = _DataService.Filter(items, Filter);
            Items.AddRange(filter, true);
        }
    }
}
