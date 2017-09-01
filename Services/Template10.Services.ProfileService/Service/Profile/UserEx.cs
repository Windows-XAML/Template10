using Windows.UI.Xaml.Media.Imaging;

namespace Template10.Services.Profile
{
    public class UserEx : IUserEx
    {
        private string _display;
        public UserEx()
        {
            // empty
        }
        internal UserEx(string display)
        {
            _display = display;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DomainName { get; set; }
        public string DomainName_DomainOnly
        {
            get
            {
                try { return DomainName?.Split('\\')[0]; }
                catch { return string.Empty; }
            }
        }
        public string DomainName_UserOnly
        {
            get
            {
                try { return DomainName?.Split('\\')[1]; }
                catch { return string.Empty; }
            }
        }
        public string AccountName { get; set; }
        public string PrincipalName { get; set; }
        public string ProviderName { get; set; }
        public string DisplayName => string.IsNullOrEmpty(_display) ? $"{FirstName} {LastName}" : _display;
        public BitmapImage ProfileImage { get; set; }
    }
}
