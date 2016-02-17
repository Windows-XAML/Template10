using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace Sample.Models
{
    public class UserCredentials : BindableBase
    {
        string _userName = default(string);
        public string UserName { get { return _userName; } set { Set(ref _userName, value); } }

        string _password = default(string);
        public string Password { get { return _password; } set { Set(ref _password, value); } }
    }
}
