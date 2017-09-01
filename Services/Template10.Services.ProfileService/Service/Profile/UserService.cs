using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml.Media.Imaging;

namespace Template10.Services.Profile
{
    public class UserService
    {
        public async Task<IUserEx> GetUserExAsync()
        {
            var user = await User.FindAllAsync();
            var current = user.FirstOrDefault();
            var props = await current.GetPropertiesAsync(new[]
            {
                KnownUserProperties.DisplayName,
                KnownUserProperties.FirstName,
                KnownUserProperties.LastName,
                KnownUserProperties.ProviderName,
                KnownUserProperties.AccountName,
                KnownUserProperties.GuestHost,
                KnownUserProperties.PrincipalName,
                KnownUserProperties.DomainName,
                KnownUserProperties.SessionInitiationProtocolUri,
            });
            var userEx = new UserEx(props[KnownUserProperties.DisplayName] as string);
            userEx.FirstName = props[KnownUserProperties.FirstName] as string;
            userEx.LastName = props[KnownUserProperties.LastName] as string;
            userEx.DomainName = props[KnownUserProperties.DomainName] as string;
            userEx.AccountName = props[KnownUserProperties.AccountName] as string;
            userEx.PrincipalName = props[KnownUserProperties.PrincipalName] as string;
            userEx.ProviderName = props[KnownUserProperties.ProviderName] as string;
            var reference = await current.GetPictureAsync(UserPictureSize.Size424x424);
            if (reference != null)
            {
                var stream = await reference.OpenReadAsync();
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(stream);
                userEx.ProfileImage = bitmapImage;
            }

            return userEx;
        }
    }
}
