using Template10.Mvvm;

namespace Template10.Samples.SearchSample.Models
{
    public class UserCredentials : BindableBase
    {
        string _userName = default(string);
        public string UserName { get { return _userName; } set { Set(ref _userName, value); } }

        string _password = default(string);
        public string Password { get { return _password; } set { Set(ref _password, value); } }
    }
}
