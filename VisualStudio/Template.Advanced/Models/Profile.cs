using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace Sample.Models
{
    public class Profile : BindableBase
    {
        string _key;
        public string Key
        {
            get => _key;
            set => Set(ref _key, value);
        }

        string _firstName;
        public string FirstName
        {
            get => _firstName;
            set => Set(ref _firstName, value);
        }

        string _lastName;
        public string LastName
        {
            get => _lastName;
            set => Set(ref _lastName, value);
        }

        string _email;
        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        string _web;
        public string Web
        {
            get => _web;
            set => Set(ref _web, value);
        }
    }
}
