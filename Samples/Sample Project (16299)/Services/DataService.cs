using Bogus;
using Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Services
{
    interface IDataService
    {
        IEnumerable<DataItem> GetItems();
    }

    class DataService : IDataService
    {
        public IEnumerable<DataItem> GetItems()
        {
            var items = new Faker<DataItem>()
                .RuleFor(i => i.Title, f => f.Lorem.Word())
                .RuleFor(i => i.Text, f => f.Lorem.Sentence());
            return items.Generate(50);
        }
    }
}
