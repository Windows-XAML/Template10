using Prism.Mvvm;
using Prism.Navigation;
using Sample.Models;
using Sample.Services;
using Template10.SampleData.StarTrek;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;
using System;

namespace Sample.ViewModels
{
    class MainPageViewModel : BindableBase, INavigatedAwareAsync
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;

        public MainPageViewModel(IDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName.Equals(nameof(SearchString)))
                {
                    await FillMembersAsync();
                }
            };
            return FillMembersAsync();
        }

        public ObservableCollection<GroupedMembers> Members { get; } = new ObservableCollection<GroupedMembers>();

        string _searchString = string.Empty;
        public string SearchString
        {
            get => _searchString;
            set => SetProperty(ref _searchString, value);
        }

        public async void ItemClick(object sender, Windows.UI.Xaml.Controls.ItemClickEventArgs e)
        {
            if (e.ClickedItem is Member m)
            {
                await _navigationService.NavigateAsync(
                   path: nameof(Views.ItemPage),
                   infoOverride: new DrillInNavigationTransitionInfo(),
                   parameters: (nameof(Member), m.ToJson()));
            }
        }

        private async Task FillMembersAsync()
        {
            Members.Clear();
            foreach (var group in await GetGroupsAsync())
            {
                if (group.Members.Any())
                {
                    Members.Add(group);
                }
            }

            async Task<IEnumerable<GroupedMembers>> GetGroupsAsync()
            {
                await _dataService.OpenAsync();
                return _dataService.Shows
                    .OrderBy(x => x.Ordinal)
                    .Select(x => new GroupedMembers
                    {
                        Show = x,
                        Members = GetFilteredMembers(x)
                    });
            }

            ObservableCollection<Member> GetFilteredMembers(Show show)
            {
                var members = _dataService.Members
                    .Where(x => x.Show == show.Abbreviation)
                    .Where(x => x.Character.ToLower().Contains(SearchString.Trim().ToLower())
                                || x.Actor.ToLower().Contains(SearchString.Trim().ToLower()));
                return new ObservableCollection<Member>(members);
            }
        }
    }
}
