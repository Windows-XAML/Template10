using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sample.Models
{
    public class GroupedMembers
    {
        public SampleData.StarTrek.Show Show { get; set; }
        public ObservableCollection<SampleData.StarTrek.Member> Members { get; set; }
    }
}
