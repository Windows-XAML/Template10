using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

namespace Minimal.ViewModels
{
    public class DetailPageViewModel : Minimal.Mvvm.ViewModelBase
    {
        Services.SecondaryTilesService.SecondaryTileService _SecondaryTileService;

        public DetailPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // designtime data
                this.Value = "Designtime value";
                return;
            }
            else
            {
                _SecondaryTileService = new Services.SecondaryTilesService.SecondaryTileService();
            }
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (state.Any())
            {
                // use cache value(s)
                if (state.ContainsKey(nameof(Value))) Value = state[nameof(Value)]?.ToString();
                // clear any cache
                state.Clear();
            }
            else
            {
                // use navigation parameter
                Value = string.Format("You passed '{0}'", parameter?.ToString());
            }
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                // persist into cache
                state[nameof(Value)] = Value;
            }
            return base.OnNavigatedFromAsync(state, suspending);
        }

        public override void OnNavigatingFrom(NavigatingEventArgs args)
        {
            args.Cancel = false;
        }

        private string _Value = "Default";
        public string Value { get { return _Value; } set { Set(ref _Value, value); } }

        Services.SecondaryTilesService.PinCommand _pinCommand;
        public Services.SecondaryTilesService.PinCommand PinCommand
        {
            get
            {
                if (_pinCommand != null)
                    return _pinCommand;
                return _pinCommand = new Services.SecondaryTilesService.PinCommand
                {
                    Pin = () => _SecondaryTileService.Pin(this),
                    Unpin = () => _SecondaryTileService.UnPin(this),
                    IsPinned = _SecondaryTileService.IsPinned(this),
                };
            }
        }
    }
}
