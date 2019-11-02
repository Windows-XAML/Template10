using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Navigation;
using Windows.Storage;

namespace Template10.NuGet.FileActivationSample.ViewModels
{
    class MarkdownPageViewModel : ViewModelBase
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public override async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            Title = "The application was launched by file association.";
            var file = parameters.GetValue<IStorageItem>("file");
            if (file.IsOfType(StorageItemTypes.File))
            {
                Text = await FileIO.ReadTextAsync(file as StorageFile);
            }
        }
    }
}
