using System;
using Windows.UI.Xaml;

namespace Sample.ViewModels
{
    public class SettingsPageViewModel : Sample.Mvvm.ViewModelBase
    {
        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();
        public AboutPartViewModel AboutPartViewModel { get; } = new AboutPartViewModel();
    }
}
