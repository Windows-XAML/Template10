using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;

namespace Sample.ViewModels
{
    public class DetailPageViewModel : Template10.Mvvm.ViewModelBase
    {
        Services.MessageService.MessageService _MessageService;

        public DetailPageViewModel()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _MessageService = new Services.MessageService.MessageService();
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Message = _MessageService.GetMessage(parameter.ToString());
        }

        Models.Message _Message = default(Models.Message);
        public Models.Message Message { get { return _Message; } set { Set(ref _Message, value); } }
    }
}
