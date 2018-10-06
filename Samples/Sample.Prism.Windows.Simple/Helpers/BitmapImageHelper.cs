using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
namespace Samples.Helpers
{
    public static class BitmapImageHelper
    {
        public static async Task<StorageFile> SaveBitmapImageToFile(UIElement content, string name)
        {
            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(content);

            var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();
            var file = await ApplicationData.Current.TemporaryFolder
                .CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
            using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, fileStream);
                encoder.SetPixelData(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Ignore,
                    (uint)renderTargetBitmap.PixelWidth,
                    (uint)renderTargetBitmap.PixelHeight,
                    DisplayInformation.GetForCurrentView().LogicalDpi,
                    DisplayInformation.GetForCurrentView().LogicalDpi,
                    pixelBuffer.ToArray());
                await encoder.FlushAsync();
            }
            return file;
        }

        public static async Task<BitmapImage> ReadBitmapImageFromFile(Size size, StorageFile file)
        {
            using (var fileStream = await file.OpenAsync(FileAccessMode.Read))
            {
                var bitmapImage = new BitmapImage
                {
                    DecodePixelHeight = (int)size.Height,
                    DecodePixelWidth = (int)size.Width,
                };
                await bitmapImage.SetSourceAsync(fileStream);
                return bitmapImage;
            }
        }
    }
}
