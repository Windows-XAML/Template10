using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Repositories
{
    class FavoritesRepository
    {
        const string CACHEKEY = "Colors-Favorites";
        Services.FileService.FileService _fileService;
        Services.ColorService.ColorService _colorService;

        public FavoritesRepository()
        {
            _fileService = new Services.FileService.FileService(); ;
            _colorService = new Services.ColorService.ColorService();
        }

        private List<Models.ColorInfo> _favorites;
        public async Task<List<Models.ColorInfo>> GetColorsAsync()
        {
            _favorites  = _favorites ?? (_favorites = (await _fileService.ReadColorsAsync(CACHEKEY)) ?? new List<Models.ColorInfo>());

            // because SolidColorBrush is not serializable, we need to rebuild it
            foreach (var item in _favorites)
                item.ContrastForegroundBrush = new Windows.UI.Xaml.Media.SolidColorBrush(_colorService.GetContrast(item.Color));

            if (FavoritesChanged != null)
                FavoritesChanged.Invoke();
            return _favorites;
        }

        public async Task Add(Models.ColorInfo color)
        {
            if (color == null)
                return;
            _favorites = _favorites ?? await GetColorsAsync();
            if (_favorites.Any(x => x.ToString().Equals(color.ToString())))
                return;
            _favorites.Add(color);

            // save changes
            await _fileService.WriteColors(CACHEKEY, _favorites);

            if (FavoritesChanged != null)
                FavoritesChanged.Invoke();
        }

        public async Task Remove(Models.ColorInfo color)
        {
            if (color == null)
                return;
            _favorites = _favorites ?? await GetColorsAsync();
            _favorites.RemoveAll(x => x.ToString().Equals(color.ToString()));

            // save changes
            await _fileService.WriteColors(CACHEKEY, _favorites);

            if (FavoritesChanged != null)
                FavoritesChanged.Invoke();
        }

        public Action FavoritesChanged { get; set; }
    }
}
