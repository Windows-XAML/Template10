using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sample.Models;

namespace Sample.Services.DataService
{
    public class DataService : IDataService<DataItem>
    {
        private static List<DataItem> Cache;
        private List<DataItem> GetCache()
        {
            if (Cache == null)
            {
                Cache = new List<DataItem>();
                FillWithSampleData(ref Cache);
            }
            return Cache;
        }

        public async Task<DataItem> AddAsync(DataItem item)
        {
            await Task.CompletedTask;
            var cache = GetCache();
            cache.Add(item);
            return item;
        }

        public async Task<DataItem> DeleteAsync(DataItem item)
        {
            await Task.CompletedTask;
            var cache = GetCache();
            cache.Remove(item);
            return item;
        }

        public async Task<IReadOnlyList<DataItem>> LoadAsync()
        {
            await Task.CompletedTask;
            // TODO: load data
            var cache = GetCache();
            return cache.AsReadOnly();
        }

        public async Task<bool> SaveAsync()
        {
            await Task.CompletedTask;
            // TODO: save data
            return true;
        }

        public IReadOnlyList<DataItem> Filter(IReadOnlyList<DataItem> items, string title)
        {
            var cache = GetCache();
            var results = items
                .Where(x => string.IsNullOrEmpty(title) || Regex.IsMatch(x.Title, title, RegexOptions.IgnoreCase));
            return results.ToList().AsReadOnly();
        }

        private void FillWithSampleData(ref List<DataItem> cache)
        {
            var items = new[] { "Apples", "Blackberries", "Cherries", "Coconuts", "Figs", "Grapefruits", "Grapes", "Kiwis", "Lemons", "Nectarines", "Oranges", "Peaches", "Pears", "Plumbs", "Raspberries", "Strawberries", "Cabbage", "Carrots", "Cauliflower", "Egg Plant", "Ginger", "Green Beans", "Kale", "Onions", "Tomatoes", "Zuchinnis" };
            foreach (var item in items.OrderBy(x => x))
            {
                cache.Add(new DataItem
                {
                    Image = $"https://raw.githubusercontent.com/Windows-XAML/Template10/master/Assets/Samples/{item}.jpg",
                    Title = item,
                    Detail = $"Lorem ipsum dolor sit amet, consectetur {item} adipiscing elit. Ut eu lectus semper est vehicula lobortis eget a magna. Nullam venenatis, diam nec vulputate semper, {item} dolor arcu scelerisque sem, id viverra massa libero accumsan eros. Praesent semper tempus ultricies. In {item} ultricies efficitur rutrum. Fusce at lorem velit. Nullam ut magna nulla. "
                });
            };
        }
    }
}
