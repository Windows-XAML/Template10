using Template10.Models;

namespace Template10.ViewModels
{
    public class TodoItemViewModel : Mvvm.ViewModelBase
    {
        public TodoItemViewModel(Models.Todo todo)
        {
            this.Todo = todo;
        }

        public Models.Todo Todo { get; private set; }

        public string Title
        {
            get { return Todo.Title; }
            set { Todo.Title = value; base.RaisePropertyChanged(); }
        }

        public Models.States State
        {
            get { return Todo.State; }
            set { Todo.State = value; base.RaisePropertyChanged(); }
        }
    }
}
