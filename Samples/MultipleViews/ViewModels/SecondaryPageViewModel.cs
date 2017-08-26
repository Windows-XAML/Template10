using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Template10.Common;
using Windows.UI.ViewManagement;

namespace Sample.ViewModels
{
    public class SecondaryPageViewModel:BaseViewModel
    {
        private readonly ApplicationView view = ApplicationView.GetForCurrentView();

        public ICommand CloseAndWaitCommand { get; }


        public SecondaryPageViewModel()
        {
            CloseAndWaitCommand = new RelayCommand(CloseAndWait);
        }

        private async void CloseAndWait()
        {
            SendMessage("Start closing");
            var task = Task.Delay(10000);
            await
                ApplicationViewSwitcher.SwitchAsync(WindowWrapper.Default().ApplicationView().Id, view.Id,
                    ApplicationViewSwitchingOptions.ConsolidateViews);
            SendMessage("Hid view");
            SendMessage("Waiting for task to end...");
            await task;
            SendMessage("Async task ended");
        }

        private void SendMessage(string message)
        {
            MessengerInstance.Send(new GenericMessage<string>($"{DateTime.Now} - {view.Title}:{view.Id}: {message}"));
        }
    }
}
