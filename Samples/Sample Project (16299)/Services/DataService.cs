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
            Randomizer.Seed = new Random(8675309);

            var images = new[] { "Apples", "Bananas", "Blackberries", "Cabbage", "Carrots", "Cauliflower", "Cherries", "Coconuts", "EggPlant", "Figs", "Ginger", "Grapefruits", "Grapes", "GreenBeans", "Kale", "Kiwis", "Lemons", "Nectarines", "Onions", "Oranges", "Peaches", "Pears", "Plumbs", "Raspberries", "Strawberries", "Tomatoes", "Zuchinnis" };

            var items = new Faker<DataItem>()
                .RuleFor(i => i.Index, f => ++f.IndexVariable)
                .RuleFor(i => i.Title, f => f.Commerce.ProductName())
                .RuleFor(i => i.Text, f => f.Lorem.Sentences(3))
                .RuleFor(i => i.Image, f => $"ms-appx:///Images/{f.PickRandom(images)}.jpg");
            return items.Generate(150);
        }
    }
}
