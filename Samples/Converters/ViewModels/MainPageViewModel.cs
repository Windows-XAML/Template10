using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.ViewModels
{
    class MainPageViewModel : Template10.Mvvm.ViewModelBase
    {
        public static MainPageViewModel Instance { get; private set; }

        public MainPageViewModel()
        {
            Instance = this;

            NumberValues = new ObservableCollection<double>(
                Enumerable.Range(0, 40).Select(i => i * 0.25463105308788066303)
            );

            Random rand = new Random();
            NumberValue = NumberValues[rand.Next(NumberValues.Count - 1)];
            BooleanValue = false;
            DateTimeValue = DateTime.UtcNow;
        }

        public ObservableCollection<double> NumberValues { get; set; }

        private DateTime _DateTimeValue;
        public DateTime DateTimeValue
        {
            get
            {
                return _DateTimeValue;
            }
            set { Set(ref _DateTimeValue, value); }
        }

        private Double _NumberValue;
        public Double NumberValue
        {
            get
            {
                return _NumberValue;
            }
            set { Set(ref _NumberValue, value); }
        }

        private Boolean _BooleanValue;
        public Boolean BooleanValue
        {
            get
            {
                return _BooleanValue;
            }
            set { Set(ref _BooleanValue, value); }
        }
    }
}
