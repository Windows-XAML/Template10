using Sample.Models;
using SampleData.Food;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Services
{
    public class DataService : IDataService
    {
        private Database _database;

        public DataService() => _database = new Database();

        public IEnumerable<Group<Fruit>> GetGroups()
        {
            yield return new Group<Fruit> { Title = "Odd Items", Items = GetItems().Where((x, i) => i % 2 != 0) };
            yield return new Group<Fruit> { Title = "Event Items", Items = GetItems().Where((x, i) => i % 2 == 0) };
        }

        public IEnumerable<Fruit> GetItems() => _database.Fruit;

        public IEnumerable<string> GetSuggestions(string text, int count) => GetItems()
                .Where(x => x.Name.ToLower().Contains(text.ToLower()))
                .OrderBy(x => x.Name)
                .Take(count)
                .Select(x => x.Name);

        public Task<bool> InitializeAsync() => _database.OpenAsync();

        public IEnumerable<Fruit> Search(string text) => GetItems()
                .Where(x => x.Name.ToLower().Contains(text.ToLower()))
                .OrderBy(x => x.Name);
    }
}
