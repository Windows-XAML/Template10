using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Models
{
    public class TodoItem
    {
        public string Key { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public DateTime DueDate { get; set; } = DateTime.Now;
        public States State { get; set; } = States.NotStarted;
    }
}
