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
        ColorRepository _colorRepository;
        FavoritesRepository _favoritesRepository;
        NavigationService _navigationService;

        public MainPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                LoadDesigntimeData();
            }
            else
            {
                _colorRepository = new Repositories.ColorRepository();
                _favoritesRepository = new Repositories.FavoritesRepository();
                _navigationService = (App.Current as App).NavigationService;
            }
        }

        private void LoadDesigntimeData()
        {
            var color = new Models.ColorInfo()
            {
                Name = "Sample",
                Color = Windows.UI.Colors.Green,
            };
            for (int i = 0; i < 10; i++)
            {
                foreach (var item in new[] { this.Favorites, this.Yellows, this.Reds, this.Greens, this.Blues })
                    item.Add(color);
            }
            foreach (var item in new[] { this.Yellows, this.Reds, this.Greens, this.Blues })
            {
                item.First().ColSpan = 3;
                item.First().RowSpan = 2;
            }
        }

        public override void OnNavigatedTo(string parameter, NavigationMode mode, IDictionary<string, object> state)
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
        public Models.ColorInfo Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
                if (value != null)
                    this.SelectedFavorite = null;
            }
        }

        private Models.ColorInfo _selectedFavorite;
        public Models.ColorInfo SelectedFavorite
        {
            get { return _selectedFavorite; }
            set
            {
                Set(ref _selectedFavorite, value);
                if (value != null)
                    this.Selected = null;
            }
        }

        private bool _busy;
        public bool Busy { get { return _busy; } set { Set(ref _busy, value); } }

        private Mvvm.Command _openCommand;
        public Mvvm.Command OpenCommand
        {
            get
            {
                return _openCommand ?? (_openCommand = new Mvvm.Command(() =>
                {
                    if (this.Selected != null)
                        _navigationService.Navigate(typeof(Views.DetailPage), Selected.Name);
                    if (this.SelectedFavorite != null)
                        _navigationService.Navigate(typeof(Views.DetailPage), SelectedFavorite.Name);
                },
                () =>
                {
                    if (this.Selected != null)
                        return true;
                    else if (this.SelectedFavorite != null)
                        return true;
                    return false;
                }));
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
                    if (!this.Favorites.Any(x => x.ToString().Equals(this.Selected.ToString())))
                        this.Favorites.Add(this.Selected.Clone());
                },
                () =>
                {
                    if (this.Selected == null)
                        return false;
                    else
                        return !this.Favorites.Any(x => x.ToString().Equals(this.Selected.ToString()));
                }));
            }
        }

        private Mvvm.Command _unFavoriteCommand;
        public Mvvm.Command UnFavoriteCommand
        {
            get
            {
                return _unFavoriteCommand ?? (_unFavoriteCommand = new Mvvm.Command(async () =>
                {
                    await _favoritesRepository.Remove(this.SelectedFavorite);
                    this.Favorites.Remove(this.SelectedFavorite);
                },
                () => { return this.SelectedFavorite != null; }));
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
                            this.Favorites.Add(color.Clone());
                    }
                    finally { this.Busy = false; }
                },
                () => { return !this.Busy; }));
            }
        }
    }
}
