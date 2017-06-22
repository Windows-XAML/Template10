using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Common;
using Template10.Portable.Navigation;
using Template10.Services.NavigationService;

namespace Sample.ViewModels
{
    public class DetailPageViewModel : ViewModelBase
    {
        public DetailPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Value = "Designtime value";
            }
        }

        private string _Value = "Default";
        public string Value { get { return _Value; } set { Set(ref _Value, value); } }

        public async override Task OnNavigatedToAsync(INavigatedToParameters parameter)
        {
            if (parameter.Resuming)
            {
                var item = await parameter.ToNavigationInfo.PageState.TryGetValueAsync<string>(nameof(Value));
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
                await parameters.PageState.SetValueAsync(nameof(Value), Value);
            }
        }
    }
}
