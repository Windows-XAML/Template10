using Bogus;
using Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Services
{
    public class FakeDataService : IDataService
    {
        public async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public IEnumerable<DataGroup> GetGroups(int groups, int items)
        {
            Randomizer.Seed = new Random(8675309);

            var data = new Faker<DataGroup>()
                .RuleFor(i => i.Title, f => $"{++f.IndexVariable}. {f.Lorem.Sentence()}")
                .RuleFor(i => i.Items, f => GetItems(items));
            return data.Generate(groups);
        }

        public IEnumerable<DataItem> GetItems(int items)
        {
            Randomizer.Seed = new Random(8675309);

            var images = new[] { "Apples", "Bananas", "Blackberries", "Cabbage", "Carrots", "Cauliflower", "Cherries", "Coconuts",
                "EggPlant", "Figs", "Ginger", "Grapefruits", "Grapes", "GreenBeans", "Kale", "Kiwis", "Lemons", "Nectarines",
                "Onions", "Oranges", "Peaches", "Pears", "Plumbs", "Raspberries", "Strawberries", "Tomatoes", "Zuchinnis" };

            var data = new Faker<DataItem>()
                .RuleFor(i => i.Id, f => ++f.IndexVariable)
                .RuleFor(i => i.Title, f => f.Commerce.ProductName())
                .RuleFor(i => i.Text, f => f.Lorem.Sentences(3))
                .RuleFor(i => i.Image, f => $"ms-appx:///Images/{f.PickRandom(images)}.jpg");
            return data.Generate(items);
        }

        public IEnumerable<string> GetSuggestions(string text, int count)
        {
            return GetItems(150)
                .Where(x => x.Title.ToLower().Contains(text.ToLower()))
                .OrderBy(x => x.Title)
                .Take(count)
                .Select(x => x.Title);
        }

        public IEnumerable<DataItem> Search(string text)
        {
            return GetItems(150)
                .Where(x => x.Title.ToLower().Contains(text.ToLower()))
                .OrderBy(x => x.Title);
        }

        public void Save(DataItem item)
        {
            // TODO
        }

        public void Revert(DataItem item)
        {
            // TODO
        }

        public void Delete(DataItem item)
        {
            // TODO
        }
    }
}
