using System;
using Template10.Models;

namespace Template10.ViewModels
{
    public class TodoItemViewModel : Mvvm.ViewModelBase
    {
        public TodoItemViewModel(Models.TodoItem todo)
        {
            this.Todo = todo;
        }

        public Models.TodoItem Todo { get; private set; }

        public string Title
        {
            get { return Todo.Title; }
            set { Todo.Title = value; base.RaisePropertyChanged(); }
        }

        public DateTime DueDate
        {
            get { return Todo.DueDate; }
            set { Todo.DueDate = value; base.RaisePropertyChanged(); }
        }

        public Models.States State
        {
            get { return Todo.State; }
            set { Todo.State = value; base.RaisePropertyChanged(); }
        }
    }
}
