using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Template10.Utils
{
    public static class InkUtils
    {
        public async static Task SaveAsync(this InkCanvas inkCanvas, string fileName, StorageFolder folder = null)
        {
            folder = folder ?? ApplicationData.Current.TemporaryFolder;
            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            if (inkCanvas.InkPresenter.StrokeContainer.GetStrokes().Any())
            {
                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await inkCanvas.InkPresenter.StrokeContainer.SaveAsync(stream);
                }
            }
        }

        public async static Task LoadAsync(this InkCanvas inkCanvas, string fileName, StorageFolder folder = null)
        {
            folder = folder ?? ApplicationData.Current.TemporaryFolder;
            var file = await folder.TryGetItemAsync(fileName) as StorageFile;
            if (file != null)
            {
                try
                {
                    using (var stream = await file.OpenSequentialReadAsync())
                    {
                        await inkCanvas.InkPresenter.StrokeContainer.LoadAsync(stream);
                    }
                }
                catch { }
            }
        }

        public static void Clear(this InkCanvas inkCanvas)
        {
            inkCanvas.InkPresenter.StrokeContainer.Clear();
        }

        public static async Task<string> Recognize(this InkCanvas inkCanvas)
        {
            var strokes = inkCanvas.InkPresenter.StrokeContainer;
            if (strokes.GetStrokes().Any())
            {
                var recognizer = new InkRecognizerContainer();
                var results = await recognizer.RecognizeAsync(strokes, InkRecognitionTarget.All);
                var candidates = results.Select(x => x.GetTextCandidates().First());
                return string.Join(" ", candidates);
            }
            else
            {
                return string.Empty;
            }
        }

        //public class InkBitmapRenderer
        //{
        //    public async Task<SoftwareBitmap> RenderAsync(IEnumerable<InkStroke> inkStrokes, double width, double height)
        //    {
        //        var dpi = DisplayInformation.GetForCurrentView().LogicalDpi;
        //        try
        //        {
        //            var renderTarget = new CanvasRenderTarget(_canvasDevice, (float)width, (float)height, dpi); using (renderTarget)
        //            {
        //                using (var drawingSession = renderTarget.CreateDrawingSession())
        //                {
        //                    drawingSession.DrawInk(inkStrokes);
        //                }

        //                return await SoftwareBitmap.CreateCopyFromSurfaceAsync(renderTarget);
        //            }
        //        }
        //        catch (Exception e) when (_canvasDevice.IsDeviceLost(e.HResult))
        //        {
        //            _canvasDevice.RaiseDeviceLost();
        //        }

        //        return null;
        //    }

        //    private void HandleDeviceLost(CanvasDevice sender, object args)
        //    {
        //        if (sender == _canvasDevice)
        //        {
        //            RecreateDevice();
        //        }
        //    }

        //    private void RecreateDevice()
        //    {
        //        _canvasDevice.DeviceLost -= HandleDeviceLost;

        //        _canvasDevice = CanvasDevice.GetSharedDevice(_canvasDevice.ForceSoftwareRenderer); _canvasDevice.DeviceLost += HandleDeviceLost;
        //    }
        //}

        //private async Task RenderBitmap(this InkCanvas inkCanvas)
        //{
        //    var strokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();

        //    var renderer = new RenderTargetBitmap();

        //    var renderedImage = await _inkBitmapRenderer.RenderAsync(
        //        strokes,
        //        inkCanvas.ActualWidth,
        //        inkCanvas.ActualHeight);

        //    var convertedImage = SoftwareBitmap
        //        .Convert(renderedImage,
        //           BitmapPixelFormat.Bgra8,
        //           BitmapAlphaMode.Premultiplied);

        //    if (renderedImage != null)
        //    {
        //        // Convert to a format appropriate for SoftwareBitmapSource.
        //        var convertedImage = SoftwareBitmap.Convert(
        //            renderedImage,
        //            BitmapPixelFormat.Bgra8,
        //            BitmapAlphaMode.Premultiplied
        //            );
        //        await InkImageSource.SetBitmapAsync(convertedImage);

        //        var renderTargetBitmap = new RenderTargetBitmap();
        //        var currentDpi = DisplayInformation.GetForCurrentView().LogicalDpi;

        //        // Prepare for RenderTargetBitmap by hiding the InkCanvas and displaying the                // rasterized strokes instead.
        //        ImageInkCanvas.Visibility = Visibility.Collapsed;
        //        InkImage.Visibility = Visibility.Visible;

        //        await renderTargetBitmap.RenderAsync(InkingRoot);
        //        var pixelData = await renderTargetBitmap.GetPixelsAsync();

        //        // Restore the original layout now that we have created the RenderTargetBitmap image.
        //        ImageInkCanvas.Visibility = Visibility.Visible;
        //        InkImage.Visibility = Visibility.Collapsed;

        //        // Create destination file for the new image
        //        var destFolder = await SettingsService.GetStorageFolderForPhotoFile(ViewModel.CurrentPhoto.PhotoId);
        //        var file = await destFolder.CreateFileAsync($"{Guid.NewGuid().ToString("D")}.png",
        //                CreationCollisionOption.GenerateUniqueName);

        //        using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
        //        {
        //            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
        //            encoder.SetPixelData(
        //                BitmapPixelFormat.Bgra8,
        //                BitmapAlphaMode.Ignore,
        //                (uint)renderTargetBitmap.PixelWidth,
        //                (uint)renderTargetBitmap.PixelHeight,
        //                currentDpi,
        //                currentDpi,
        //                pixelData.ToArray()
        //                );

        //            await encoder.FlushAsync();
        //        }

        //        // Update the SightFile in the database
        //        await ViewModel.UpdatePhotoImageUriAsync(file.GetUri());

        //        // Erase all strokes.
        //        ImageInkCanvas.InkPresenter.StrokeContainer.Clear();
        //    }
        //}
    }
}
