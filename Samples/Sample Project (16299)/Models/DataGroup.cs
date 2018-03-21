using System.Collections.Generic;

namespace Sample.Models
{
    public class DataGroup
    {
        public string Title { get; set; }
        public IEnumerable<DataItem> Items { get; set; }
    }
}
