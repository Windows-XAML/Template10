using Template10.Samples.ConvertersSample.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Template10.Samples.ConvertersSample.ViewModels
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

            SimpleModels = new ObservableCollection<SimpleModel>();
            for (int i = 0; i < 100; i++)
            {
                SimpleModels.Add(new SimpleModel());
            }
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


        private byte? _byteValue = null;
        /// <summary>
        /// Sets and gets the ByteValue property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public byte? ByteValue
        {
            get { return _byteValue; }
            set { Set(ref _byteValue, value); }
        }


        private int _intValue = 0;
        /// <summary>
        /// Sets and gets the IntValue property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int IntValue
        {
            get { return _intValue; }
            set { Set(ref _intValue, value); }
        }

        private DateTimeOffset _dateTimeOffsetValue = DateTime.Today;
        public DateTimeOffset DateTimeOffsetValue
        {
            get
            {
                return _dateTimeOffsetValue;
            }
            set { Set(ref _dateTimeOffsetValue, value); }
        }


        private Decimal _decimalValue = 500.25M;
        /// <summary>
        /// Sets and gets the MyProperty property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Decimal DecimalValue
        {
            get { return _decimalValue; }
            set { Set(ref _decimalValue, value); }
        }



        private ObservableCollection<SimpleModel> _simpleModels = null;
        /// <summary>
        /// Sets and gets the MyProperty property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<SimpleModel> SimpleModels
        {
            get { return _simpleModels; }
            set { Set(ref _simpleModels, value); }
        }



        private SimpleModel _selectedSimpleModel = null;
        /// <summary>
        /// Sets and gets the SelectedSimpleModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public SimpleModel SelectedSimpleModel
        {
            get { return _selectedSimpleModel; }
            set { Set(ref _selectedSimpleModel, value); }
        }

    }
}
