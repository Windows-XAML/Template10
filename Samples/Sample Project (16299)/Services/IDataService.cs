using Sample.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Services
{
    public interface IDataService
    {
        Task InitializeAsync();
        IEnumerable<DataGroup> GetGroups(int groups, int items);
        IEnumerable<DataItem> GetItems(int items);
        IEnumerable<string> GetSuggestions(string text, int count);
        IEnumerable<DataItem> Search(string text);
        void Save(DataItem item);
        void Revert(DataItem item);
        void Delete(DataItem item);
    }
}
