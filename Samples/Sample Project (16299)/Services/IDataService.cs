using Sample.Models;
using Template10.SampleData.Food;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Services
{
    public interface IDataService
    {
        Task<bool> InitializeAsync();
        IEnumerable<Group<Fruit>> GetGroups();
        List<Fruit> GetItems();
        IEnumerable<string> GetSuggestions(string text, int count);
        IEnumerable<Fruit> Search(string text);
        void Save(Fruit item);
        void Delete(Fruit item);
        void Revert(Fruit item);
    }
}
