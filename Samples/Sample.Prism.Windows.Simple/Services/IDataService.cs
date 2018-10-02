using Template10.SampleData.StarTrek;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Services
{
    interface IDataService
    {
        IEnumerable<Show> Shows { get; }

        IEnumerable<Member> Members { get; }

        Task OpenAsync();
        Member Find(string character);
    }
}
