using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;

namespace Sample.Views
{
    public sealed partial class Busy : UserControl, INotifyPropertyChanged
    {
        public Busy()
        {
            InitializeComponent();
        }

        string _BusyText = default(string);
        public string BusyText { get { return _BusyText; } set { Set(ref _BusyText, value); } }

        bool _IsBusy = default(bool);
        public bool IsBusy { get { return _IsBusy; } set { Set(ref _IsBusy, value); } }

        public event PropertyChangedEventHandler PropertyChanged;

        void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                RaisePropertyChanged(propertyName);
            }
        }

        void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
