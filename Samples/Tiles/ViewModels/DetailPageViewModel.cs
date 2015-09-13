﻿using Sample.Services.TileService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Sample.ViewModels
{
    public class DetailPageViewModel : Template10.Mvvm.ViewModelBase
    {
        public DetailPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // designtime data
                this.Value = "Designtime value";
                return;
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
                Value = parameter?.ToString();
            }

            UpdatePins();
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

        #region Tile

        TileService _TileSerivice = new TileService();

        public async void Pin() { await _TileSerivice.PinAsync(this); UpdatePins(); }
        public async void UnPin() { await _TileSerivice.UnPinAsync(this); UpdatePins(); }

        async void UpdatePins()
        {
            var isPinned = await _TileSerivice.IsPinned(this);
            PinVisibility = (isPinned) ? Visibility.Collapsed : Visibility.Visible;
            UnPinVisibility = (isPinned) ? Visibility.Visible : Visibility.Collapsed;
        }

        Visibility _PinVisibility = default(Visibility);
        public Visibility PinVisibility { get { return _PinVisibility; } set { Set(ref _PinVisibility, value); } }

        Visibility _UnPinVisibility = default(Visibility);
        public Visibility UnPinVisibility { get { return _UnPinVisibility; } set { Set(ref _UnPinVisibility, value); } }

        #endregion  
    }
}
