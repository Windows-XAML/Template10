using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sample.Models
{
    public class DataGroup: BindableBase
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public ObservableCollection<DataItem> Items { get; set; }
    }
}
