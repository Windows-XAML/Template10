using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Template10.ViewModels
{
    public class MainPageViewModel : Mvvm.ViewModelBase
    {
        Repositories.GroupRepository _groupRepository;
        Repositories.PersonRepository _personRepository;

        public MainPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                this.Groups = new ObservableCollection<Models.Group>(_groupRepository.Sample());
                this.SelectedGroup = this.Groups.First();
                this.People = new ObservableCollection<Models.Person>(_personRepository.Sample());
                this.SelectedPerson = this.People.First();
            }
        }

        public override async void OnNavigatedTo(string parameter, NavigationMode mode, Dictionary<string, object> state)
        {
            var groups = await _groupRepository.GetAsync();
            this.SelectedGroup = groups.FirstOrDefault();
            this.Groups.Clear();
            foreach (var group in groups)
                this.Groups.Add(group);

            var people = await _personRepository.GetAsync();
            this.SelectedPerson = people.FirstOrDefault();
            this.People.Clear();
            foreach (var person in people)
                this.People.Add(person);
        }

        public ObservableCollection<Models.Group> Groups { get; set; } = new ObservableCollection<Models.Group>();
        public ObservableCollection<Models.Person> People { get; set; } = new ObservableCollection<Models.Person>();

        public Models.Group SelectedGroup { get; set; }
        public Models.Person SelectedPerson { get; set; }
    }
}
