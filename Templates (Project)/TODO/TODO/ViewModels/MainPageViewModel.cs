using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
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
                this.SelectedTodoList = this.TodoLists.First();
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

        public override void OnNavigatedTo(string parameter, NavigationMode mode, Dictionary<string, object> state)
        {
            LoadCommand.Execute(null);
        }

        public override void OnNavigatedFrom(Dictionary<string, object> state, bool suspending)
        {
            SaveCommand.Execute(null);
        }

        #region Properties

        bool _busy = false;
        public bool Busy { get { return _busy; } set { Set(ref _busy, value); } }

        private ObservableCollection<ViewModels.TodoListViewModel> _TodoLists = new ObservableCollection<TodoListViewModel>();
        public ObservableCollection<ViewModels.TodoListViewModel> TodoLists { get { return _TodoLists; } private set { Set(ref _TodoLists, value); } }

        private static IEnumerable<Models.States> _stateOptions = Enum.GetValues(typeof(Models.States)).Cast<Models.States>();
        public IEnumerable<Models.States> StateOptions { get { return _stateOptions; } }

        private ViewModels.TodoListViewModel _SelectedTodoList = default(ViewModels.TodoListViewModel);
        public ViewModels.TodoListViewModel SelectedTodoList { get { return _SelectedTodoList; } set { Set(ref _SelectedTodoList, value); SelectedTodoListIsSelected = (value != null); } }

        private bool _SelectedTodoListIsSelected = default(bool);
        public bool SelectedTodoListIsSelected { get { return _SelectedTodoListIsSelected; } private set { Set(ref _SelectedTodoListIsSelected, value); } }

        #endregion

        #region Commands

        Mvvm.Command _AddListCommand = default(Mvvm.Command);
        public Mvvm.Command AddListCommand { get { return _AddListCommand ?? (_AddListCommand = new Mvvm.Command(ExecuteAddListCommand, CanExecuteAddListCommand)); } }
        private bool CanExecuteAddListCommand() { return !Busy; }
        private void ExecuteAddListCommand()
        {
            try
            {
                var index = this.TodoLists.IndexOf(this.SelectedTodoList);
                var item = new ViewModels.TodoListViewModel(_todoListRepository.Factory(title: "New List"));
                this.TodoLists.Insert((index > -1) ? index : 0, item);
                this.SelectedTodoList = item;
                SaveCommand.Execute(null);
            }
            catch { this.SelectedTodoList = null; }
        }

        Mvvm.Command _RemoveListCommand = default(Mvvm.Command);
        public Mvvm.Command RemoveListCommand { get { return _RemoveListCommand ?? (_RemoveListCommand = new Mvvm.Command(ExecuteRemoveListCommand, CanExecuteRemoveListCommand)); } }
        private bool CanExecuteRemoveListCommand() { return !Busy && this.SelectedTodoList != null; }
        private void ExecuteRemoveListCommand()
        {
            try
            {
                var index = this.TodoLists.IndexOf(this.SelectedTodoList);
                this.TodoLists.Remove(this.SelectedTodoList);
                this.SelectedTodoList = this.TodoLists[index];
                SaveCommand.Execute(null);
            }
            catch { this.SelectedTodoList = null; }
        }

        Mvvm.Command _LoadCommand = default(Mvvm.Command);
        public Mvvm.Command LoadCommand { get { return _LoadCommand ?? (_LoadCommand = new Mvvm.Command(ExecuteLoadCommand, CanExecuteLoadCommand)); } }
        private bool CanExecuteLoadCommand() { return !Busy; }
        private async void ExecuteLoadCommand()
        {
            try
            {
                Busy = true;
                await Task.Delay(2000);
                var data = _todoListRepository.Sample(3).Select(x => new ViewModels.TodoListViewModel(x));
                this.TodoLists.Clear();
                foreach (var item in data.OrderBy(x => x.TodoList.Title))
                {
                    this.TodoLists.Add(item);
                }
            }
            finally { Busy = false; }
        }

        Mvvm.Command _SaveCommand = default(Mvvm.Command);
        public Mvvm.Command SaveCommand { get { return _SaveCommand ?? (_SaveCommand = new Mvvm.Command(ExecuteSaveCommand, CanExecuteSaveCommand)); } }
        private bool CanExecuteSaveCommand() { return true; }
        private async void ExecuteSaveCommand()
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
