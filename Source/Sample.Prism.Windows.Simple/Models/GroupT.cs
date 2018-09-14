using System.Collections.Generic;

namespace Sample.Models
{
    public class GroupT<T>
    {
        public string Title { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
