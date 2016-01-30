using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Samples.MasterDetail.Views;
using Template10.Common;
using Template10.Mvvm;

namespace Samples.MasterDetail.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        Services.MessageService.MessageService _messageService;

        public MainPageViewModel()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _messageService = new Services.MessageService.MessageService();
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Messages = _messageService.GetMessages();
            Selected = Messages?.FirstOrDefault();
            return Task.CompletedTask;
        }

        ObservableCollection<Models.Message> _messages = default(ObservableCollection<Models.Message>);
        public ObservableCollection<Models.Message> Messages { get { return _messages; } private set { Set(ref _messages, value); } }

        string _searchText = default(string);
        public string SearchText { get { return _searchText; } set { Set(ref _searchText, value); } }

        public DelegateCommand SwitchToControlCommand =
            new DelegateCommand(() => BootStrapper.Current.NavigationService.Navigate(typeof (MasterDetailsPage)));

        Models.Message _selected = default(Models.Message);
        public object Selected
        {
            get { return _selected; }
            set
            {
                var message = value as Models.Message;
                Set(ref _selected, message);
                if (message != null)
                    message.IsRead = true;
            }
        }
    }
}
