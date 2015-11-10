using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Sample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        Services.MessageService.MessageService _messageService;

        public MainPageViewModel()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _messageService = new Services.MessageService.MessageService();
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Messages = _messageService.GetMessages();
            Selected = Messages.First();
        }

        ObservableCollection<Models.Message> _messages = default(ObservableCollection<Models.Message>);
        public ObservableCollection<Models.Message> Messages { get { return _messages; } private set { Set(ref _messages, value); } }

        string _searchText = default(string);
        public string SearchText { get { return _searchText; } set { Set(ref _searchText, value); } }

        Models.Message _selected = default(Models.Message);
        public Models.Message Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
                if (value != null)
                    value.IsRead = true;
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        DelegateCommand _deleteCommand;
        public DelegateCommand DeleteCommand => _deleteCommand ?? (_deleteCommand = new DelegateCommand(() =>
                                                              {
                                                                  if (Selected != null)
                                                                  {
                                                                      _messageService.DeleteMessage(Selected);
                                                                      Selected = null;
                                                                  }
                                                              }, () => { return Selected != null; }));

        DelegateCommand _searchCommand;
        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(() =>
                                                              {
                                                                  Messages = _messageService.Search(SearchText);
                                                              }));

        DelegateCommand _clearCommand;
        public DelegateCommand ClearCommand => _clearCommand ?? (_clearCommand = new DelegateCommand(() =>
                                                             {
                                                                 Messages = _messageService.Search(SearchText = string.Empty);
                                                             }));
    }
}
