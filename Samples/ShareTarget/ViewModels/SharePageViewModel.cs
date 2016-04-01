using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Template10.Mvvm;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;

namespace Template10.Samples.ShareTargetSample.ViewModels
{
    public class SharePageViewModel : ViewModelBase
    {
        public void Complete()
        {
            ShareOperation.ReportCompleted();
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var key = parameter as string;
            if (key != null && SessionState.ContainsKey(key))
            {
                ShareOperation = SessionState.Get<ShareOperation>(key);
            }
            else
            {
                Content = "Opened without using share";
                return;
            }

            try
            {
                if (this.ShareOperation.Data.Contains(StandardDataFormats.Html))
                { Content = (await ShareOperation.Data.GetHtmlFormatAsync()).ToString(); }
                else if (ShareOperation.Data.Contains(StandardDataFormats.Text))
                { Content = (await ShareOperation.Data.GetTextAsync()).ToString(); }
                else if (ShareOperation.Data.Contains(StandardDataFormats.WebLink))
                { Content = (await ShareOperation.Data.GetWebLinkAsync()).AbsoluteUri; }
                else if (ShareOperation.Data.Contains(StandardDataFormats.ApplicationLink))
                { Content = (await ShareOperation.Data.GetApplicationLinkAsync()).AbsoluteUri; }
                else if (ShareOperation.Data.Contains(StandardDataFormats.Bitmap))
                {
                    Content = nameof(StandardDataFormats.Bitmap);
                    var bitmap = await ShareOperation.Data.GetBitmapAsync();
                    using (var stream = await bitmap.OpenReadAsync())
                    {
                        Bitmap = new BitmapImage();
                        Bitmap.SetSource(stream);
                    }
                }
                else if (ShareOperation.Data.Contains(StandardDataFormats.StorageItems))
                {
                    Content = nameof(StandardDataFormats.StorageItems);
                    foreach (var item in await ShareOperation.Data.GetStorageItemsAsync())
                    {
                        Content += item.Path + Environment.NewLine;
                    }
                }
                else
                {
                    Content = "Some unsupported share type.";
                    return;
                }

                QuickLink = ShareOperation.QuickLinkId ?? "None set";

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
            catch (Exception e) { Content = e.Message; }

        }

        string _Content = default(string);
        public string Content { get { return _Content; } set { Set(ref _Content, value); } }

        string _QuickLink = default(string);
        public string QuickLink { get { return _QuickLink; } set { Set(ref _QuickLink, value); } }

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

