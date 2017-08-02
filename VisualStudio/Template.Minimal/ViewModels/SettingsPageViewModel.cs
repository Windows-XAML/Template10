using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml;

namespace Sample.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public override Task OnNavigatedToAsync(INavigatedToParameters parameter)
        {
            return Task.CompletedTask;
        }

        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();
        public AboutPartViewModel AboutPartViewModel { get; } = new AboutPartViewModel();
    }

    public class SettingsPartViewModel : BindableBase
    {
        Services.SettingsServices.SettingsService _settings;

        public SettingsPartViewModel() => _settings = Services.SettingsServices.SettingsService.GetInstance();

        public bool UseShellBackButton
        {
            get => _settings.UseShellBackButton;
            set => Set(() => { _settings.UseShellBackButton = value; });
        }

        public bool UseLightThemeButton
        {
            get => _settings.AppTheme.Equals(ApplicationTheme.Light);
            set => Set(() => { _settings.AppTheme = value ? ApplicationTheme.Light : ApplicationTheme.Dark; });
        }

        private string _BusyText = "Please wait...";
        public string BusyText
        {
            get => _BusyText;
            set
            {
                Set(ref _BusyText, value);
                _ShowBusyCommand.RaiseCanExecuteChanged();
            }
        }

        DelegateCommand _ShowBusyCommand;
        public DelegateCommand ShowBusyCommand
            => _ShowBusyCommand ?? (_ShowBusyCommand = new DelegateCommand(async () =>
            {
                Views.Busy.SetBusy(true, _BusyText);
                await Task.Delay(5000);
                Views.Busy.SetBusy(false);
            }, () => !string.IsNullOrEmpty(BusyText)));

        public void Set(Action set, [CallerMemberName]string propertyName = null)
        {
            set.Invoke();
            RaisePropertyChanged(propertyName);
        }
    }

    public class AboutPartViewModel : BindableBase
    {
        public Uri Logo => Windows.ApplicationModel.Package.Current.Logo;

        public string DisplayName => Windows.ApplicationModel.Package.Current.DisplayName;

        public string Publisher => Windows.ApplicationModel.Package.Current.PublisherDisplayName;

        public string Version
        {
            get
            {
                var version = Windows.ApplicationModel.Package.Current.Id.Version;
                return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
        }

        public Uri RateMe => new Uri($"ms-windows-store:review?PFN={Uri.EscapeUriString(Windows.ApplicationModel.Package.Current.Id.FamilyName)}");
    }
}
