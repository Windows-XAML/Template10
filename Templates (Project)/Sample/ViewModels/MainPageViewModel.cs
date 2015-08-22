using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.ApplicationModel.Store;
using Windows.UI.Xaml.Navigation;

namespace Template10.ViewModels
{
    public class MainPageViewModel : Mvvm.ViewModelBase
    {
        Repositories.TodoListRepository _todoListRepository;

        public MainPageViewModel()
        {
            _todoListRepository = new Repositories.TodoListRepository();

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // designtime sample data
                var data = _todoListRepository.Sample().Select(x => new ViewModels.TodoListViewModel(x));
                this.TodoLists = new ObservableCollection<ViewModels.TodoListViewModel>(data);
            }
            else
            {
                // update commands
                this.PropertyChanged += (s, e) =>
                {
                    this.AddListCommand.RaiseCanExecuteChanged();
                    this.RemoveListCommand.RaiseCanExecuteChanged();
                };
            }
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            LoadCommand.Execute(null);
        }

        public override async void OnNavigatingFrom(NavigatingEventArgs args)
        {
            await ExecuteSaveCommand();
        }

        bool _busy = false;
        public bool Busy { get { return _busy; } set { Set(ref _busy, value); } }

        private ObservableCollection<ViewModels.TodoListViewModel> _TodoLists = new ObservableCollection<TodoListViewModel>();
        public ObservableCollection<ViewModels.TodoListViewModel> TodoLists { get { return _TodoLists; } private set { Set(ref _TodoLists, value); } }

        private ViewModels.TodoListViewModel _SelectedTodoList = default(ViewModels.TodoListViewModel);
        public ViewModels.TodoListViewModel SelectedTodoList { get { return _SelectedTodoList; } set { Set(ref _SelectedTodoList, value); } }

        #region Commands

        DelegateCommand _AddListCommand = default(DelegateCommand);
        public DelegateCommand AddListCommand { get { return _AddListCommand ?? (_AddListCommand = new DelegateCommand(ExecuteAddListCommand, CanExecuteAddListCommand)); } }
        private bool CanExecuteAddListCommand() { return !Busy; }
        private void ExecuteAddListCommand()
        {
            try
            {
                var item = new ViewModels.TodoListViewModel(_todoListRepository.Factory(title: "New List"));
                this.TodoLists.Insert(0, item);
                this.SelectedTodoList = item;
                SaveCommand.Execute(null);
            }
            catch { }
        }

        DelegateCommand<ViewModels.TodoListViewModel> _RemoveListCommand = default(DelegateCommand<ViewModels.TodoListViewModel>);
        public DelegateCommand<ViewModels.TodoListViewModel> RemoveListCommand { get { return _RemoveListCommand ?? (_RemoveListCommand = new DelegateCommand<ViewModels.TodoListViewModel>(ExecuteRemoveListCommand, CanExecuteRemoveListCommand)); } }
        private bool CanExecuteRemoveListCommand(ViewModels.TodoListViewModel list) { return !Busy && list != null; }
        private void ExecuteRemoveListCommand(ViewModels.TodoListViewModel list)
        {
            try
            {
                var index = this.TodoLists.IndexOf(list);
                this.TodoLists.Remove(list);
                SaveCommand.Execute(null);
            }
            catch { }
        }

        DelegateCommand _LoadCommand = default(DelegateCommand);
        public DelegateCommand LoadCommand { get { return _LoadCommand ?? (_LoadCommand = new DelegateCommand(ExecuteLoadCommand, CanExecuteLoadCommand)); } }
        private bool CanExecuteLoadCommand() { return !Busy; }
        private async void ExecuteLoadCommand()
        {
            try
            {
                Busy = true;
                await Task.Delay(2000);
                var data = _todoListRepository.Sample(10).Select(x => new ViewModels.TodoListViewModel(x));
                this.TodoLists.Clear();
                foreach (var item in data.OrderBy(x => x.TodoList.Title))
                {
                    this.TodoLists.Add(item);
                }
            }
            finally { Busy = false; }
        }

        DelegateCommand _SaveCommand = default(DelegateCommand);
        public DelegateCommand SaveCommand { get { return _SaveCommand ?? (_SaveCommand = new DelegateCommand(async () => { await ExecuteSaveCommand(); }, CanExecuteSaveCommand)); } }
        private bool CanExecuteSaveCommand() { return true; }
        private async Task ExecuteSaveCommand()
        {
            while (Busy)
            {
                await Task.Delay(100);
            }
            try
            {
                Busy = true;
                await Task.Delay(2000);
                await _todoListRepository.SaveAsync(this.TodoLists.Select(x => x.TodoList).ToList());
            }
            finally { Busy = false; }
        }

        #endregion
    }
}
