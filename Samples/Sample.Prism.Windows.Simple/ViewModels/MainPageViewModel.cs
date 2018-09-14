using Prism.Mvvm;
using Prism.Navigation;
using Sample.Models;
using SampleData.StarTrek;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.ViewModels
{
    class MainPageViewModel : BindableBase, INavigatedAwareAsync
    {
        static Database _data;

        static MainPageViewModel()
        {
            _data = new Database();
        }

        public MainPageViewModel()
        {
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals(nameof(SearchString)))
                {
                    FillMembers();
                }
            };
        }

        public async Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            await _data.OpenAsync();
            FillMembers();
        }

        private void FillMembers()
        {
            Members.Clear();
            foreach (var group in _data.Shows
                .OrderBy(x => x.Ordinal)
                .Select(x => new GroupedMembers { Show = x } ))
            {
                var members = GetFilteredMembers(group.Show);
                group.Members = new ObservableCollection<Member>(members);
                if (group.Members.Any())
                {
                    Members.Add(group);
                }
            }
        }

        private IEnumerable<Member> GetFilteredMembers(Show show)
        {
            return _data.Members
                .Where(x => x.Show == show.Abbreviation)
                .Where(x => x.Character.ToLower().Contains(SearchString.Trim().ToLower()) || x.Actor.ToLower().Contains(SearchString.Trim().ToLower()));
        }

        public ObservableCollection<GroupedMembers> Members { get; } = new ObservableCollection<GroupedMembers>();

        string _searchString = string.Empty;
        public string SearchString
        {
            get => _searchString;
            set => SetProperty(ref _searchString, value);
        }
    }
}
