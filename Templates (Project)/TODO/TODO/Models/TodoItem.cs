using System;

namespace Template10.Models
{
    public class TodoItem : Mvvm.BindableBase
    {
        public string Key { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public DateTime DueDate { get; set; } = DateTime.Now;
        public States State { get; set; } = States.NotStarted;
    }
}
