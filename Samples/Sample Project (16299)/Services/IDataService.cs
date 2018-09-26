using Sample.Models;
using SampleData.Food;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Services
{
    public interface IDataService
    {
        Task<bool> InitializeAsync();
        IEnumerable<Group<Fruit>> GetGroups();
        IEnumerable<Fruit> GetItems();
        IEnumerable<string> GetSuggestions(string text, int count);
        IEnumerable<Fruit> Search(string text);
    }
}
