using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Template10.Models
{
    public class TodoList : Mvvm.BindableBase
    {
        public string Key { get; set; } = Guid.NewGuid().ToString();

        private string _title;
        public string Title { get { return _title; } set { Set(ref _title, value); } }

        private ObservableCollection<Models.TodoItem> _items = new ObservableCollection<TodoItem>();
        public ObservableCollection<Models.TodoItem> Items { get { return _items; } set { Set(ref _items, value); } }
    }
}
