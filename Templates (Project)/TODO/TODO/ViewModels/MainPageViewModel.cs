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
        Repositories.TodoRepository _todoRepository;

        public MainPageViewModel()
        {
            _todoRepository = new Repositories.TodoRepository();

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                this.Todos = new ObservableCollection<ViewModels.TodoItemViewModel>(_todoRepository.Sample().Select(x => new ViewModels.TodoItemViewModel(x)));
                this.SelectedTodo = this.Todos.First();
            }
            else
            {
                this.PropertyChanged += (s, e) =>
                {
                    this.AddCommand.RaiseCanExecuteChanged();
                    this.RemoveCommand.RaiseCanExecuteChanged();
                };
            }
        }

        public override async void OnNavigatedTo(string parameter, NavigationMode mode, Dictionary<string, object> state)
        {
            await Load();
        }

        public ObservableCollection<ViewModels.TodoItemViewModel> Todos { get; } = new ObservableCollection<ViewModels.TodoItemViewModel>();

        ViewModels.TodoItemViewModel _selectedTodo = default(ViewModels.TodoItemViewModel);
        public ViewModels.TodoItemViewModel SelectedTodo { get { return _selectedTodo; } set { Set(ref _selectedTodo, value); } }

        bool _busy = false;
        public bool Busy { get { return _busy; } set { Set(ref _busy, value); } }

        Command _AddCommand;
        public Command AddCommand { get { return _AddCommand ?? (_AddCommand = new Command(ExecuteAddCommand, CanExecuteAddCommand)); } }
        private bool CanExecuteAddCommand() { return true; }
        private async void ExecuteAddCommand()
        {
            var todo = _todoRepository.Factory(title: "New Task");
            this.Todos.Add(new ViewModels.TodoItemViewModel(todo));
            await Save();
        }

        Command<ViewModels.TodoItemViewModel> _RemoveCommand;
        public Command<ViewModels.TodoItemViewModel> RemoveCommand { get { return _RemoveCommand ?? (_RemoveCommand = new Command<ViewModels.TodoItemViewModel>(ExecuteRemoveCommand, CanExecuteRemoveCommand)); } }
        private bool CanExecuteRemoveCommand(ViewModels.TodoItemViewModel todo) { return todo != null; }
        private async void ExecuteRemoveCommand(ViewModels.TodoItemViewModel todo)
        {
            this.Todos.Remove(todo);
            await Save();
        }

        async Task Load()
        {
            if (!Busy)
            {
                try
                {
                    Busy = true;
                    this.Todos.Clear();
                    foreach (var todo in _todoRepository.Sample())
                        // foreach (var todo in await _todoRepository.GetAsync())
                        this.Todos.Add(new ViewModels.TodoItemViewModel(todo));
                    this.SelectedTodo = this.Todos.FirstOrDefault();
                }
                finally { Busy = false; }
            }
        }

        async Task Save()
        {
            while (Busy)
                await Task.Delay(100);
            try
            {
                Busy = true;
                await _todoRepository.SaveAsync(this.Todos.Select(x => x.Todo).ToList());
            }
            finally { Busy = false; }
        }
    }
}
