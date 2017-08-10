using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Template10.Services.Profile
{
    public abstract class Bindable : INotifyPropertyChanged
    {
        internal Bindable()
        {
            // empty
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                RaisePropertyChanged(propertyName);
            }
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
