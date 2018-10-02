using Template10.SampleData.StarTrek;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Services
{
    public class DataService : IDataService
    {
        private Database _data;

        public DataService() => _data = new Database();

        public Task OpenAsync() => _data.OpenAsync();

        public IEnumerable<Show> Shows => _data.Shows;

        public IEnumerable<Member> Members => _data.Members;

        public Member Find(string character) => Members.FirstOrDefault(x => Equals(x.Character, character));
    }
}
