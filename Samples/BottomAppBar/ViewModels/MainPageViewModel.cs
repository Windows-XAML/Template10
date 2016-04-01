using System;
using System.Linq;
using Template10.Mvvm;
using System.Collections.ObjectModel;

namespace Template10.Samples.BottomAppBarSample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel()
        {
            string up = "";
            string down = "";
            var random = new Random((int)DateTime.Now.Ticks);
            foreach (var item in Enumerable.Range(1, 20))
            {
                var val = random.Next(-10, 10);
                var data = new Models.DataItem
                {
                    Value1 = $"Product name {1}",
                    Value2 = random.Next(10, 55).ToString("C2"),
                    Value3 = (val * .01).ToString("C2"),
                    Arrow = val > 0 ? up : down,
                    Value4 = (random.Next(-10, 10) * .01).ToString("P"),
                    Value5 = DateTime.Now.AddDays(random.Next(10, 1000)).ToString("dd MMM yyyy"),
                    Value6 = "Template 10",
                };
                Items.Add(data);
            }
        }

        public ObservableCollection<Models.DataItem> Items { get; } = new ObservableCollection<Models.DataItem>();
    }
}
