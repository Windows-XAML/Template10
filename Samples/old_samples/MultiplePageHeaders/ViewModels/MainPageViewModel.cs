using System;
using System.Collections.ObjectModel;
using System.Linq;
using Template10.Controls;
using Template10.Mvvm;
using Windows.UI.Xaml;

namespace MultiplePageHeaders.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel()
        {
            Items.AddRange(Enumerable.Range(1, 10).Select(x => new MyModel { Text = x.ToString() }));
            Items.ItemPropertyChanged += (s, e) =>
            {
                var model = e.ChangedItem as MyModel;
                model.Special = $"{e.PropertyChangedArgs.PropertyName} changed to {model.Text}";
            };
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            timer.Tick += (s, e) =>
            {
                foreach (var item in Items)
                {
                    item.Text = Guid.NewGuid().ToString();
                }
            };
            timer.Start();
        }

        public class MyModel : BindableBase
        {
            string _Text = "No text";
            public string Text { get { return _Text; } set { Set(ref _Text, value); } }

            string _Special = "Nothing special";
            public string Special { get { return _Special; } set { Set(ref _Special, value); } }
        }
        public ObservableItemCollection<MyModel> Items { get; } = new ObservableItemCollection<MyModel>();

        public void Test() =>
            NavigationService.Navigate(typeof(Views.ContainerPage));

        public void GotoSettings() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 2);
    }
}

