using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Template10.Samples.MessagingSample.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var evt = App.EventAggregator.GetEvent<Messages.UpdateDateTimeMessage>();
            evt.Subscribe(d => Value = d);
            return Task.CompletedTask;
        }

        DateTime _Value = default(DateTime);
        public DateTime Value { get { return _Value; } set { Set(ref _Value, value); } }
    }
}
