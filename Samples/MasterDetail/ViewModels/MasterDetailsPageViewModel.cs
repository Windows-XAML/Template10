using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Samples.MasterDetail.Models;
using Samples.MasterDetail.Views;
using Template10.Common;
using Template10.Mvvm;
using Template10.Utils;

namespace Samples.MasterDetail.ViewModels
{
    public class MasterDetailsPageViewModel : ViewModelBase
    {
        Services.MessageService.MessageService _messageService;

        public MasterDetailsPageViewModel()
        {
            Messages = new ObservableCollection<Message>();
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _messageService = new Services.MessageService.MessageService();
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            RefreshCommand.Execute();
            return Task.CompletedTask;
        }

        ObservableCollection<Models.Message> _messages = default(ObservableCollection<Models.Message>);

        public ObservableCollection<Models.Message> Messages
        {
            get { return _messages; }
            private set { Set(ref _messages, value); }
        }

        string _searchText = default(string);

        public string SearchText
        {
            get { return _searchText; }
            set { Set(ref _searchText, value); }
        }

        public readonly DelegateCommand SwitchToPageCommand =
            new DelegateCommand(() => BootStrapper.Current.NavigationService.Navigate(typeof (MainPage)));

        public DelegateCommand RefreshCommand => new DelegateCommand(() =>
        {
            IsMasterLoading = true;
            Messages.Clear();
            Selected = null;
            WindowWrapper.Current().Dispatcher.Dispatch(() =>
            {
                Messages.AddRange(_messageService.GetMessages());
                Selected = Messages?.FirstOrDefault();
                IsMasterLoading = false;
            }, 2000);
        });

        Message _selected = default(Message);

        public object Selected
        {
            get { return _selected; }
            set
            {
                var message = value as Message;
                Set(ref _selected, message);
                NextCommand.RaiseCanExecuteChanged();
                PreviousCommand.RaiseCanExecuteChanged();
                if (message == null) return;
                message.IsRead = true;
                IsDetailsLoading = true;
                WindowWrapper.Current().Dispatcher.Dispatch(() =>
                {
                    IsDetailsLoading = false;
                }, 1000);
            }
        }

        private DelegateCommand _nextCommand;

        public DelegateCommand NextCommand
        {
            get
            {
                return _nextCommand ??
                    (_nextCommand = new DelegateCommand(ExecuteNextCommand, CanExecuteNextCommand));
            }
            set { Set(ref _nextCommand, value); }
        }

        private void ExecuteNextCommand()
        {
            if (Selected == null)
                return;
            var index = Messages.IndexOf(_selected);
            if (index == -1)
                return;
            var next = index + 1;
            Selected = Messages[next];
        }

        private bool CanExecuteNextCommand()
        {
            if (Selected == null)
                return false;
            var index = Messages.IndexOf(_selected);
            if (index == -1)
                return false;
            return index < (Messages.Count - 1);
        }

        private DelegateCommand _previousCommand;

        public DelegateCommand PreviousCommand
        {
            get
            {
                return _previousCommand ??
                       (_previousCommand = new DelegateCommand(ExecutePreviousCommand, CanExecutePreviousCommand));
            }
            set { Set(ref _previousCommand, value); }
        }

        private bool CanExecutePreviousCommand()
        {
            if (Selected == null)
                return false;
            var index = Messages.IndexOf(_selected);
            return index > 0;
        }

        private void ExecutePreviousCommand()
        {
            if (Selected == null)
                return;
            var index = Messages.IndexOf(_selected);
            if (index == -1)
                return;
            var previous = index - 1;
            Selected = Messages[previous];
        }

        private bool _isDetailsLoading;

        public bool IsDetailsLoading
        {
            get { return _isDetailsLoading; }
            set { Set(ref _isDetailsLoading, value); }
        }

        private bool _isMasterLoading;

        public bool IsMasterLoading
        {
            get { return _isMasterLoading; }
            set { Set(ref _isMasterLoading, value); }
        }
    }
}