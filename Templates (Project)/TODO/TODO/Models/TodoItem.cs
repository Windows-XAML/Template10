using System;

namespace Template10.Models
{
    public class TodoItem : Mvvm.BindableBase
    {
        private string _Key = default(string);
        public string Key { get { return _Key; } set { Set(ref _Key, value); } }

        private string _Title = default(string);
        public string Title { get { return _Title; } set { Set(ref _Title, value); } }

        private DateTime _DueDate = default(DateTime);
        public DateTime DueDate { get { return _DueDate; } set { Set(ref _DueDate, value); } }

        private States _State = default(States);
        public States State { get { return _State; } set { Set(ref _State, value); } }
    }
}
