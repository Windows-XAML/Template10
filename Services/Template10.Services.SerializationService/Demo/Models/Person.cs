using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationService.Demo.Models
{
    public class Person : 
        INotifyPropertyChanged
    {
        public Person()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Id = Guid.Empty.ToString();
                Name = "John Doe";
                Age = 30;
            }
            else
            {
                Id = Guid.NewGuid().ToString();
                Name = "";
                Age = 10;
            }
        }

        private string _Id = default(string);
        public string Id { get { return _Id; } set { _Id = value; OnPropertyChanged(nameof(Id)); } }

        private string _Name = default(string);
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged(nameof(Name)); }
        }

        private int _Age = default(int);
        public int Age
        {
            get { return _Age; }
            set { _Age = value; OnPropertyChanged(nameof(Age)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        
    }
}
