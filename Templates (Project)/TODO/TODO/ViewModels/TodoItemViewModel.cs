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

        private Models.TodoItem _todoItem;
        public Models.TodoItem TodoItem { get { return _todoItem; } set { base.Set(ref _todoItem, value); } }

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
<<<<<<< HEAD

        private static IEnumerable<Models.States> stateOptions;
        public IEnumerable<Models.States> StateOptions
        {
            get { return stateOptions ?? (stateOptions = Enum.GetValues(typeof(Models.States)).Cast<Models.States>()); }
        }
=======
>>>>>>> origin/master
    }
}
