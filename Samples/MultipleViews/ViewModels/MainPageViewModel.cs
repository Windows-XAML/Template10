using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Sample.Views;
using Template10.Common;
using Template10.Services.ViewService;

namespace Sample.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly WindowWrapper wrapper = WindowWrapper.Current();
        public IList<string> Messages { get; }=new ObservableCollection<string>();
        public ICommand OpenViewCommand { get; }

        public MainPageViewModel()
        {
            OpenViewCommand = new RelayCommand(OpenView);
        }

        private async void OpenView()
        {
            var service = new ViewService();
            var control = await service.OpenAsync(typeof (SecondaryPage), null, Guid.NewGuid().ToString());
            control.Released += Control_Released;
        }

        private void Control_Released(object sender, EventArgs e)
        {
            Window.Current.Close();
            var control = (ViewLifetimeControl) sender;
            PostMessage(new GenericMessage<string>($"{DateTime.Now} - {control.Id}: Released"));
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            MessengerInstance.Register<GenericMessage<string>>(this, PostMessage);
            return base.OnNavigatedToAsync(parameter, mode, state);
        }

        private void PostMessage(GenericMessage<string> message)
        {
            wrapper.Dispatcher.Dispatch(() => Messages.Add(message.Content));
        }
    }
}
