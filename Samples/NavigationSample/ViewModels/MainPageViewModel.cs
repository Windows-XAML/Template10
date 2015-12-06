using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

namespace NavigationSample.ViewModels
{
	public class MainPageViewModel : Template10.Mvvm.ViewModelBase
	{
		public MainPageViewModel()
		{
			if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
			{
				// designtime data
				Value = "Designtime value";
				return;
			}
		}

		public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
		{
			if (parameter is int)
				Level = (int)parameter;

			if (mode == NavigationMode.Back || mode == NavigationMode.Refresh)
			{
				// use cache value(s)
				if (state.ContainsKey(nameof(Value))) Value = state[nameof(Value)]?.ToString();
				// clear any cache
				state.Clear();
			}
		}

		public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
		{
			// persist into cache
			state[nameof(Value)] = Value;

			return base.OnNavigatedFromAsync(state, suspending);
		}

		public override void OnNavigatingFrom(NavigatingEventArgs args)
		{
			base.OnNavigatingFrom(args);
		}

		private int _Level = 0;
		public int Level
		{
			get { return _Level; }
			set { Set(ref _Level, value); }
		}



		private string _Value = string.Empty;
		public string Value
		{
			get { return _Value; }
			set { Set(ref _Value, value); }
		}

		private DelegateCommand gotoNextLevelCommand;
		public DelegateCommand GotoNextLevelPageCommand => gotoNextLevelCommand ?? (gotoNextLevelCommand = new DelegateCommand(() =>
		{
			NavigationService.Navigate(typeof(Sample.Views.MainPage), Level+1);
		}));
	}
}
