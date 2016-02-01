using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Common;
using System;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;

namespace ShareTarget.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var key = parameter as StateItemKey;
            if (key != null && SessionState.Contains(key))
            {
                this.ShareOperation = SessionState.Get<ShareOperation>(key);
            }
            else
            {
                return;
            }

            try
            {
                if (this.ShareOperation.Data.Contains(StandardDataFormats.Html))
                    Comment = (await ShareOperation.Data.GetHtmlFormatAsync()).ToString();
                else if (ShareOperation.Data.Contains(StandardDataFormats.Text))
                    Comment = (await ShareOperation.Data.GetTextAsync()).ToString();
                else if (ShareOperation.Data.Contains(StandardDataFormats.WebLink))
                    Comment = (await ShareOperation.Data.GetWebLinkAsync()).AbsoluteUri;
                else if (ShareOperation.Data.Contains(StandardDataFormats.ApplicationLink))
                    Comment = (await ShareOperation.Data.GetApplicationLinkAsync()).AbsoluteUri;
                else if (ShareOperation.Data.Contains(StandardDataFormats.Bitmap))
                {
                    Comment = nameof(StandardDataFormats.Bitmap);
                    var bitmap = await ShareOperation.Data.GetBitmapAsync();
                    using (var stream = await bitmap.OpenReadAsync())
                    {
                        Bitmap = new BitmapImage();
                        Bitmap.SetSource(stream);
                    }
                }
                else if (ShareOperation.Data.Contains(StandardDataFormats.StorageItems))
                {
                    Comment = nameof(StandardDataFormats.StorageItems);
                    foreach (var item in await ShareOperation.Data.GetStorageItemsAsync())
                    {
                        Comment += item.Path + Environment.NewLine;
                    }
                }
                else
                {
                    Comment = "Opened without using share";
                }

                if (ShareOperation.Data.Properties.Square30x30Logo != null)
                {
                    using (var stream = await ShareOperation.Data.Properties.Square30x30Logo.OpenReadAsync())
                    {
                        Logo = new BitmapImage();
                        Logo.SetSource(stream);
                    }
                }

                if (ShareOperation.Data.Properties.Thumbnail != null)
                {
                    using (var stream = await ShareOperation.Data.Properties.Thumbnail.OpenReadAsync())
                    {
                        Thumbnail = new BitmapImage();
                        Thumbnail.SetSource(stream);
                    }
                }
            }
            catch (Exception e) { Comment = e.Message; }
        }

        string _Comment = default(string);
        public string Comment { get { return _Comment; } set { Set(ref _Comment, value); } }

        ShareOperation _ShareOperation = default(ShareOperation);
        public ShareOperation ShareOperation { get { return _ShareOperation; } set { Set(ref _ShareOperation, value); } }

        BitmapImage _Bitmap = default(BitmapImage);
        public BitmapImage Bitmap { get { return _Bitmap; } set { Set(ref _Bitmap, value); } }

        BitmapImage _Logo = default(BitmapImage);
        public BitmapImage Logo { get { return _Logo; } set { Set(ref _Logo, value); } }

        BitmapImage _Thumbnail = default(BitmapImage);
        public BitmapImage Thumbnail { get { return _Thumbnail; } set { Set(ref _Thumbnail, value); } }
    }
}

