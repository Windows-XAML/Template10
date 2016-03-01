using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Navigation;

namespace JumpList.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        Services.DataService _DataService;
        Services.ToastService _ToastService;
        Services.SettingService _SettingService;

        public MainPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled) { /* TODO */ }
            else
            {
                _DataService = new Services.DataService();
                _ToastService = new Services.ToastService();
                _SettingService = new Services.SettingService();
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            File = await _DataService.GetFileInfoAsync(_SettingService.Recent);
            App.FileReceived += FileReceivedHandler;
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> pageState, bool suspending)
        {
            _SettingService.Recent = File?.Ref.Path;
            App.FileReceived -= FileReceivedHandler;
            return Task.CompletedTask;
        }

        private async void FileReceivedHandler(object sender, string path)
        {
            File = await _DataService.GetFileInfoAsync(path);
        }

        private Models.FileInfo _File;
        public Models.FileInfo File { get { return _File; } set { Set(ref _File, value); } }

        public async void Save()
        {
            try
            {
                await _DataService.SaveFileInfoAsync(File);
                _ToastService.ShowToast(File, "File successfully saved.");
            }
            catch (Exception ex)
            {
                _ToastService.ShowToast(File, $"Save failed {ex.Message}");
            }
        }

        public async void Open()
        {
            // prompt a picker
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };
            picker.FileTypeFilter.Add(".txt");
            var file = await picker.PickSingleFileAsync();

            // handle result
            if (file == null)
            {
                // nothing picked
                File = null;
                await new Windows.UI.Popups.MessageDialog("No file selected.").ShowAsync();
            }
            else
            {
                // load file
                File = await _DataService.GetFileInfoAsync(file);
            }
        }
    }
}

