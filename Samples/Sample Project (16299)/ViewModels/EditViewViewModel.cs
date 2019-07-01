using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Unity;
using Sample.Models;
using Sample.Services;
using Template10.SampleData.Food;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Template10.Services.Dialog;
using Windows.UI.Xaml;

namespace Sample.ViewModels
{
    public class EditViewViewModel : BindableBase
    {
        private DataItem _item;
        public DataItem Item
        {
            get => _item;
            set => SetProperty(ref _item, value);
        }

        public ICommand SaveCommand { get; } = new DelegateCommand<Fruit>(item => DataService().Save(item));
        public ICommand DeleteCommand { get; } = new DelegateCommand<Fruit>(item => DataService().Delete(item));
        public ICommand RevertCommand { get; } = new DelegateCommand<Fruit>(item => DataService().Revert(item));

        private static IDataService DataService()
        {
            var container = (Application.Current as PrismApplication).Container;
            return container.Resolve<IDataService>();
        }
    }
}
