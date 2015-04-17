using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Template10.Repositories;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

namespace Template10.ViewModels
{
    class MainPageViewModel : Mvvm.ViewModelBase
    {
        ColorRepository _colorRepository = new Repositories.ColorRepository();
        FavoritesRepository _favoritesRepository = new Repositories.FavoritesRepository();
        NavigationService _navigationService = (App.Current as App).NavigationService;

        public MainPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                LoadDesigntimeData();
            }
        }

        private void LoadDesigntimeData()
        {
            // TODO
        }

        public override void OnNavigatedTo(string parameter, NavigationMode mode, Dictionary<string, object> state)
        {
            this.PropertyChanged += (s, e) =>
            {
                this.OpenCommand.RaiseCanExecuteChanged();
                this.FavoriteCommand.RaiseCanExecuteChanged();
                this.UnFavoriteCommand.RaiseCanExecuteChanged();
                this.RefreshCommand.RaiseCanExecuteChanged();
            };
            this.RefreshCommand.Execute(null);
        }

        public ObservableCollection<Models.ColorInfo> Favorites { get; } = new ObservableCollection<Models.ColorInfo>();
        public ObservableCollection<Models.ColorInfo> Yellows { get; } = new ObservableCollection<Models.ColorInfo>();
        public ObservableCollection<Models.ColorInfo> Reds { get; } = new ObservableCollection<Models.ColorInfo>();
        public ObservableCollection<Models.ColorInfo> Greens { get; } = new ObservableCollection<Models.ColorInfo>();
        public ObservableCollection<Models.ColorInfo> Blues { get; } = new ObservableCollection<Models.ColorInfo>();

        private Models.ColorInfo _selected;
        public Models.ColorInfo Selected { get { return _selected; } set { Set(ref _selected, value); } }

        private bool _busy;
        public bool Busy { get { return _busy; } set { Set(ref _busy, value); } }

        private Mvvm.Command _openCommand;
        public Mvvm.Command OpenCommand
        {
            get
            {
                return _openCommand ?? (_openCommand = new Mvvm.Command(() =>
                {
                    _navigationService.Navigate(typeof(Views.DetailPage), Selected.Name);
                },
                () => { return this.Selected != null; }));
            }
        }

        private Mvvm.Command _favoriteCommand;
        public Mvvm.Command FavoriteCommand
        {
            get
            {
                return _favoriteCommand ?? (_favoriteCommand = new Mvvm.Command(async () =>
                {
                    await _favoritesRepository.Add(this.Selected);
                    if (!this.Favorites.Any(x => x.Color.ToString().Equals(this.Selected.Color.ToString())))
                        this.Favorites.Add(this.Selected);
                },
                () => { return this.Selected != null; }));
            }
        }

        private Mvvm.Command _unFavoriteCommand;
        public Mvvm.Command UnFavoriteCommand
        {
            get
            {
                return _unFavoriteCommand ?? (_unFavoriteCommand = new Mvvm.Command(async () =>
                {
                    await _favoritesRepository.Remove(this.Selected);
                    if (this.Favorites.Any(x => x.Color.ToString().Equals(this.Selected.Color.ToString())))
                        this.Favorites.Remove(this.Selected);
                },
                () => { return this.Selected != null; }));
            }
        }

        private Mvvm.Command _refreshCommand;
        public Mvvm.Command RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new Mvvm.Command(async () =>
                {
                    try
                    {
                        this.Busy = true;

                        foreach (var item in new[] { this.Favorites, this.Yellows, this.Reds, this.Greens, this.Blues })
                            item.Clear();

                        // load colors

                        var take = 13;
                        var colors = (await _colorRepository.GetColorsAsync()).Where(x => x.Saturation < .95f).Where(x => x.Saturation > 0f);
                        foreach (var color in colors.Where(x => x.Hue > 10 && x.Hue <= 60).OrderBy(x => x.Hue).ThenBy(x => x.Brightness).ThenBy(x => x.Saturation).Take(take))
                            this.Yellows.Add(color);
                        foreach (var color in colors.Where(x => x.Hue > 60 && x.Hue <= 179).OrderBy(x => x.Hue).ThenBy(x => x.Brightness).ThenBy(x => x.Saturation).Take(take))
                            this.Greens.Add(color);
                        foreach (var color in colors.Where(x => x.Hue > 179 && x.Hue <= 260).OrderBy(x => x.Hue).ThenBy(x => x.Brightness).ThenBy(x => x.Saturation).Take(take))
                            this.Blues.Add(color);
                        foreach (var color in colors.Where(x => x.Hue > 260 || x.Hue <= 10).OrderBy(x => x.Hue).ThenBy(x => x.Brightness).ThenBy(x => x.Saturation).Take(take))
                            this.Reds.Add(color);
                        foreach (var item in new[] { this.Yellows, this.Reds, this.Greens, this.Blues })
                        {
                            item.First().ColSpan = 3;
                            item.First().RowSpan = 2;
                        }
                        this.Selected = (colors.Any()) ? colors.First() : default(Models.ColorInfo);

                        // load favorites

                        var favorites = await _favoritesRepository.GetColorsAsync();
                        foreach (var color in favorites)
                            this.Favorites.Add(color);
                    }
                    finally { this.Busy = false; }
                },
                () => { return !this.Busy; }));
            }
        }
    }
}
