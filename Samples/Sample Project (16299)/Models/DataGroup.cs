using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sample.Models
{
    public class Group<T>
    {
        public string Title { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
