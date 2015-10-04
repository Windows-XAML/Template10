using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Sample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        Services.MessageService.MessageService _MessageService;

        public MainPageViewModel()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _MessageService = new Services.MessageService.MessageService();
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Messages = _MessageService.GetMessages();
            Selected = Messages.First();
        }

        ObservableCollection<Models.Message> _Messages = default(ObservableCollection<Models.Message>);
        public ObservableCollection<Models.Message> Messages { get { return _Messages; } private set { Set(ref _Messages, value); } }

        string _SearchText = default(string);
        public string SearchText { get { return _SearchText; } set { Set(ref _SearchText, value); } }

        Models.Message _Selected = default(Models.Message);
        public Models.Message Selected
        {
            get { return _Selected; }
            set
            {
                Set(ref _Selected, value);
                if (value != null)
                    value.IsRead = true;
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        DelegateCommand _DeleteCommand;
        public DelegateCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand ?? (_DeleteCommand = new DelegateCommand(() =>
                {
                    if (Selected != null)
                    {
                        _MessageService.DeleteMessage(Selected);
                        Selected = null;
                    }
                }, () => { return Selected != null; }));
            }
        }

        DelegateCommand _SearchCommand;
        public DelegateCommand SearchCommand
        {
            get
            {
                return _SearchCommand ?? (_SearchCommand = new DelegateCommand(() =>
                {
                    Messages = _MessageService.Search(SearchText);
                }));
            }
        }

        DelegateCommand _ClearCommand;
        public DelegateCommand ClearCommand
        {
            get
            {
                return _ClearCommand ?? (_ClearCommand = new DelegateCommand(() =>
                {
                    Messages = _MessageService.Search(SearchText = string.Empty);
                }));
            }
        }
    }
}
