using Windows.UI.Xaml.Media.Imaging;

namespace Template10.Services.Profile
{
    public interface IUserEx
    {
        string FirstName { get; }
        string LastName { get; }
        string DomainName { get; }
        string DomainName_DomainOnly { get; }
        string DomainName_UserOnly { get; }
        string AccountName { get; }
        string PrincipalName { get; }
        string ProviderName { get; }
        string DisplayName { get; }
        BitmapImage ProfileImage { get; }
    }
}
