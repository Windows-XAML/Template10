using System;
using System.Collections.Generic;
using System.Linq;
using Template10.Models;

namespace Template10.ViewModels
{
    public class TodoItemViewModel : Mvvm.ViewModelBase
    {
        public TodoItemViewModel(Models.TodoItem todo)
        {
            this.TodoItem = todo;
        }

        public Models.TodoItem TodoItem { get; private set; }

        public string Title
        {
            get { return TodoItem.Title; }
            set { TodoItem.Title = value; base.RaisePropertyChanged(); }
        }

        public DateTime DueDate
        {
            get { return TodoItem.DueDate; }
            set { TodoItem.DueDate = value; base.RaisePropertyChanged(); }
        }

        public Models.States State
        {
            get { return TodoItem.State; }
            set
            {
                if (TodoItem.State != value)
                {
                    TodoItem.State = value;
                    base.RaisePropertyChanged();
                }
            }
        }

        public IEnumerable<Models.States> StateOptions
        {
            get { return Enum.GetValues(typeof(Models.States)).Cast<Models.States>(); }
        }
    }
}
