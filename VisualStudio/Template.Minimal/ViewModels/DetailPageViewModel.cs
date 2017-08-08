using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Navigation;

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
