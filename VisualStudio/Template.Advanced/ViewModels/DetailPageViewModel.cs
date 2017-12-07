using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10;
using Template10.Mvvm;
using Template10.Navigation;
using Template10.Services.Logging;

namespace Sample.ViewModels
{
    public class DetailPageViewModel : ViewModelBase
    {
        ILoggingService _loggingService;
        public DetailPageViewModel(ILoggingService loggingService = null)
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Value = "Designtime value";
            }
            else
            {
                _loggingService = loggingService;
            }
        }

        private string _value = "Default";
        public string Value { get { return _value; } set { Set(ref _value, value); } }

        public async override Task OnNavigatedToAsync(INavigatedToParameters parameter)
        {
            if (parameter.Resuming)
            {
                var item = await parameter.ToNavigationInfo.PageState.TryGetAsync<string>(nameof(Value));
                if (item.Success)
                {
                    Value = item.Value;
                }
            }
            else if (parameter.ToNavigationInfo.Parameter is string param && param != null)
            {
                Value = param;
            }
        }

        public async override Task OnNavigatedFromAsync(INavigatedFromParameters parameters)
        {
            if (parameters.Suspending)
            {
                await parameters.PageState.TrySetAsync(nameof(Value), Value);
            }
        }
    }
}
