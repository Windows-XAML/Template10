using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Template10.Models
{
    public class TodoList : Mvvm.BindableBase
    {
        private string _Key = Guid.NewGuid().ToString();
        public string Key { get { return _Key; } set { Set(ref _Key, value); } }

        private string _title;
        public string Title { get { return _title; } set { Set(ref _title, value); } }

        private ObservableCollection<Models.TodoItem> _Items = default(ObservableCollection<Models.TodoItem>);
        public ObservableCollection<Models.TodoItem> Items { get { return _Items; } set { Set(ref _Items, value); } }
    }
}
