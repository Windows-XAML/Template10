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
        Repositories.TodoItemRepository _todoItemRepository;

        public MainPageViewModel()
        {
            _todoItemRepository = new Repositories.TodoItemRepository();

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // designtime sample data
                this.TodoItems = new ObservableCollection<ViewModels.TodoItemViewModel>(_todoItemRepository.Sample().Select(x => new ViewModels.TodoItemViewModel(x)));
                this.SelectedTodoItem = this.TodoItems.First();
            }
            else
            {
                // update commands
                this.PropertyChanged += (s, e) =>
                {
                    this.AddCommand.RaiseCanExecuteChanged();
                    this.RemoveCommand.RaiseCanExecuteChanged();
                    this.FilterCommand.RaiseCanExecuteChanged();
                };
            }
        }

        public override async void OnNavigatedTo(string parameter, NavigationMode mode, Dictionary<string, object> state)
        {
            // initial data load
            await Load();
        }

        #region Properties

        IEnumerable<ViewModels.TodoItemViewModel> _cache;
        public ObservableCollection<ViewModels.TodoItemViewModel> TodoItems { get; } = new ObservableCollection<ViewModels.TodoItemViewModel>();

        ViewModels.TodoItemViewModel _selectedTodoItem = default(ViewModels.TodoItemViewModel);
        public ViewModels.TodoItemViewModel SelectedTodoItem { get { return _selectedTodoItem; } set { Set(ref _selectedTodoItem, value); } }

        bool _busy = false;
        public bool Busy { get { return _busy; } set { Set(ref _busy, value); } }

        #endregion  

        #region Commands

        Command _AddCommand;
        public Command AddCommand { get { return _AddCommand ?? (_AddCommand = new Command(ExecuteAddCommand, CanExecuteAddCommand)); } }
        private bool CanExecuteAddCommand() { return !Busy; }
        private async void ExecuteAddCommand()
        {
            try
            {
                var index = this.TodoItems.IndexOf(this.SelectedTodoItem);
                var todo = new ViewModels.TodoItemViewModel(_todoItemRepository.Factory(title: "New Task"));
                this.TodoItems.Insert((index > -1) ? index : 0, todo);
                this.SelectedTodoItem = todo;
                Populate();
            }
            catch { this.SelectedTodoItem = null; }
            finally { await Save(); }
        }

        Command _RemoveCommand;
        public Command RemoveCommand { get { return _RemoveCommand ?? (_RemoveCommand = new Command(ExecuteRemoveCommand, CanExecuteRemoveCommand)); } }
        private bool CanExecuteRemoveCommand() { return this.SelectedTodoItem != null && !Busy; }
        private async void ExecuteRemoveCommand()
        {
            try
            {
                var index = this.TodoItems.IndexOf(this.SelectedTodoItem);
                this.TodoItems.Remove(this.SelectedTodoItem);
                this.SelectedTodoItem = this.TodoItems[index];
                Populate();
            }
            catch { this.SelectedTodoItem = null; }
            finally { await Save(); }
        }

        Models.States _CurrentFilter = Models.States.Done;
        Command<string> _FilterCommand;
        public Command<string> FilterCommand { get { return _FilterCommand ?? (_FilterCommand = new Command<string>(ExecuteFilterCommand, CanExecuteFilterCommand)); } }
        private bool CanExecuteFilterCommand(string filter)
        {
            if (Busy)
                return false;
            Models.States state;
            Enum.TryParse<Models.States>(filter, out state);
            return !state.Equals(_CurrentFilter);
        }
        private void ExecuteFilterCommand(string filter)
        {
            IEnumerable<ViewModels.TodoItemViewModel> list;
            if (Enum.TryParse<Models.States>(filter, out _CurrentFilter))
                list = _cache.Where(x => x.State.Equals(_CurrentFilter));
            else
                list = _cache.ToArray();
            Populate(list);
            this.RaisePropertyChanged(string.Empty);
        }

        #endregion

        async Task Load()
        {
            if (!Busy)
            {
                try
                {
                    Busy = true;
                    await Task.Delay(2000);
                    _cache = _todoItemRepository.Sample(20).Select(x => new ViewModels.TodoItemViewModel(x));
                    // _cache = ((await _todoItemRepository.GetAsync()).Select(x => new ViewModels.TodoItemViewModel(x)));
                    FilterCommand.Execute(Models.States.NotStarted.ToString());
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
                await Task.Delay(2000);
                await _todoItemRepository.SaveAsync(this.TodoItems.Select(x => x.TodoItem).ToList());
            }
            finally { Busy = false; }
        }

        void Populate(IEnumerable<ViewModels.TodoItemViewModel> items = null)
        {
            items = items ?? this.TodoItems.ToArray();

            this.TodoItems.Clear();
            foreach (var item in items
                .OrderBy(x => x.State)
                .ThenByDescending(x => x.DueDate))
                this.TodoItems.Add(item);

            this.SelectedTodoItem = this.SelectedTodoItem ?? items.FirstOrDefault();
        }
    }
}
