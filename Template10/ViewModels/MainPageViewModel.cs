using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Template10.ViewModels
{
    class MainPageViewModel : Mvvm.ViewModelBase
    {
        public MainPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                LoadDesigntimeData();
            }
            else
            {
                this.PropertyChanged += (s, e) => { this.RefreshCommand.RaiseCanExecuteChanged(); };
            }
        }

        public override async void OnNavigatedTo(string parameter, NavigationMode mode, Dictionary<string, object> state)
        {
            await LoadRuntimeDataAsync();
        }

        private async void LoadDesigntimeData()
        {
            var repository = new Repositories.ColorRepository();
            this.Colors.Clear();
            foreach (var color in await repository.GetColorsAsync())
                this.Colors.Add(color);
            this.Selected = (this.Colors.Any()) ? Colors.First() : default(Models.ColorInfo);
        }

        private async Task LoadRuntimeDataAsync()
        {
            var repository = new Repositories.ColorRepository();
            this.Colors.Clear();
            foreach (var color in await repository.GetColorsAsync())
                this.Colors.Add(color);
            this.Selected = (this.Colors.Any()) ? Colors.First() : default(Models.ColorInfo);
        }

        private Models.ColorInfo _selected;
        public Models.ColorInfo Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
                if (value != null)
                {
                    this.Selected = null;
                    // navigate to details page
                    if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                    {
                        var nav = (App.Current as App).NavigationService;
                        nav.Navigate(typeof(Views.DetailPage), value.Name);
                    }
                }
            }
        }

        public ObservableCollection<Models.ColorInfo> Colors { get; } = new ObservableCollection<Models.ColorInfo>();

        private bool _busy;
        public bool Busy { get { return _busy; } set { Set(ref _busy, value); } }

        private Mvvm.Command _refreshCommand;
        public Mvvm.Command RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new Mvvm.Command(async () =>
                {
                    this.Busy = true;
                    await LoadRuntimeDataAsync();
                    this.Busy = false;
                }, () => { return !this.Busy; }));
            }
        }
    }
}
