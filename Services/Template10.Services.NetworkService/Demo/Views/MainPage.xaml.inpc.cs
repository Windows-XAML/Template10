using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Template10.Demo.NetworkService
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        #region INPC implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private bool Set<T>(ref T current, T value, [CallerMemberName]string propertyName = null)
        {
            if (object.Equals(current, value)) return false;
            current = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled) return;

            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;

            if (dispatcher.HasThreadAccess)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            else
            {
                dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }).AsTask().Wait();
            }
        }

        #endregion
    }
}
