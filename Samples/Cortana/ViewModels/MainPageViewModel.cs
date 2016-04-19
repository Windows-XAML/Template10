using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Template10.Samples.CortanaSample.ViewModels
{
    public class MainPageViewModel : Template10.Mvvm.ViewModelBase
    {
        public static MainPageViewModel Instance { get; private set; }

        Services.SpeechService.SpeechService _SpeechService;

        public MainPageViewModel()
        {
            _SpeechService = new Services.SpeechService.SpeechService();
            Instance = this;
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Value = parameter?.ToString() ?? "No value";
            try
            {
                await _SpeechService.LoadRecognizerAsync();
                _ListenCanExecute = true;
            }
            finally
            {
                ListenCommand.RaiseCanExecuteChanged();
            }
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            _SpeechService.Dispose();
            return Task.CompletedTask;
        }

        private string _Value;
        public string Value
        {
            get
            {
                return string.IsNullOrWhiteSpace(_Value) ? "No value" : _Value;
            }
            set { Set(ref _Value, value); }
        }

        public DelegateCommand _ListenCommand;
        public DelegateCommand ListenCommand => _ListenCommand ?? (_ListenCommand = new DelegateCommand(ListenExecute, ListenCanExecute));

        private bool _ListenCanExecute = false;
        private bool ListenCanExecute() => _ListenCanExecute;

        private async void ListenExecute()
        {
            try
            {
                Value = await _SpeechService.ListenAsync("Template10.Samples.CortanaSample Template10.Samples.CortanaSample", "Try saying, 'The quick brown fox jumps over the lazy dog.'");
            }
            catch (Exception ex)
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog
                        ("Failed to start speech recognition." +
                        Environment.NewLine + Environment.NewLine + "Message: " + ex.Message,
                        "Speech recognition failed");
                await messageDialog.ShowAsync();
            }
        }

        public async void Speak()
        {
            await SpeakAsync();
        }

        public async Task SpeakAsync()
        {
            await _SpeechService.SpeakAsync(Value);
        }

        public void GotoSettings()
        {
            NavigationService.Navigate(typeof(Views.SettingsPage));
        }
    }
}
